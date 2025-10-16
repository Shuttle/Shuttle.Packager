<template>
  <v-card flat>
    <v-card-title class="sv-card-title">
      <sv-title :title="$t('projects')" />
      <div class="sv-strip">
        <v-btn :icon="mdiFileReplaceOutline" size="x-small" @click="load"></v-btn>
        <v-btn :icon="mdiRefresh" size="x-small" @click="refresh"></v-btn>
        <v-text-field v-model="search" density="compact" :label="$t('search')" :prepend-inner-icon="mdiMagnify"
          variant="solo-filled" flat hide-details single-line></v-text-field>
        <v-select v-model="packageSource" :items="packageSources" item-title="name" item-value="name" density="compact"
          hide-details class="max-w-64" />
        <v-btn-toggle v-model="packageOptions.configuration" variant="outlined" group density="compact">
          <v-btn value="Debug">
            {{ $t("debug") }}
          </v-btn>
          <v-btn value="Release">
            {{ $t("release") }}
          </v-btn>
        </v-btn-toggle>
      </div>
    </v-card-title>
    <v-divider></v-divider>
    <v-data-table :items="projects" :headers="headers" v-model:expanded="expanded" @click:row="toggle" :mobile="null"
      mobile-breakpoint="md" v-model:search="search" :loading="busy" v-model="selected" show-select
      item-selectable="selectable" item-value="id" show-expand>
      <template v-slot:item.data-table-expand="{ internalItem, isExpanded, toggleExpand }">
        <v-btn v-if="!!internalItem.raw.log" :append-icon="isExpanded(internalItem) ? mdiChevronUp : mdiChevronDown"
          :text="isExpanded(internalItem) ? t('close') : t('show-log')" class="text-none"
          :class="internalItem.raw.status === 'failed' ? 'text-orange-400' : ''"
          :prepend-icon="internalItem.raw.status === 'failed' ? mdiAlert : undefined" size="small" slim
          @click.stop="toggleExpand(internalItem)"></v-btn>
      </template>
      <template v-slot:header.action="">
        <div class="sv-strip my-2">
          <v-btn :icon="mdiPlay" size="x-small" @click="build()"></v-btn>
          <v-btn :icon="mdiPlayBoxOutline" size="x-small" @click="pack()"></v-btn>
          <v-btn :icon="mdiPlayNetworkOutline" size="x-small" @click="push()"></v-btn>
        </div>
      </template>
      <template v-slot:item.action="{ item }">
        <div class="sv-strip my-2">
          <v-btn v-if="item.selectable" :icon="mdiPlay" size="x-small" @click.stop="build(item)"
            :disabled="item.busy"></v-btn>
          <v-btn v-if="item.selectable" :icon="mdiPlayBoxOutline" size="x-small" @click="pack(item)"
            :disabled="item.busy"></v-btn>
          <v-btn v-if="item.selectable" :icon="mdiPlayNetworkOutline" size="x-small" @click="push(item)"
            :disabled="item.busy"></v-btn>
          <v-btn :icon="mdiApplicationOutline" size="x-small" @click="open(item)"></v-btn>
          <v-btn :icon="mdiOpenInNew" size="x-small" :href="`https://www.nuget.org/packages/${item.name}`"
            target="_blank"></v-btn>
        </div>
      </template>
      <template v-slot:item.name="{ item }">
        <div class="sv-strip my-2">
          <span>{{ item.name }}</span>
        </div>
        <v-progress-linear v-if="item.busy" indeterminate />
      </template>
      <template v-slot:item.version="{ item }">
        <div class="sv-strip my-2" v-if="item.editingVersion">
          <form @submit.prevent="setVersion(item)" class="sv-strip w-64">
            <v-btn :icon="mdiCloseCircleOutline" size="x-small" @click.stop="cancelVersion(item)"></v-btn>
            <v-btn :icon="mdiCheckCircleOutline" size="x-small" @click.stop="setVersion(item)"></v-btn>
            <v-text-field v-model="item.vnext" hide-details variant="solo-filled" density="compact"
              @click.stop></v-text-field>
          </form>
        </div>
        <span v-else class="cursor-pointer" @click.stop="openVersion(item)">{{ item.version }}</span>
      </template>
      <template #expanded-row="{ columns, item }">
        <tr>
          <td :colspan="columns.length">
            <div class="sv-expand-container font-mono wrap-anywhere bg-neutral-900 text-neutral-300">
              <pre class="whitespace-pre-wrap">{{ item.log }}</pre>
            </div>
          </td>
        </tr>
      </template>
    </v-data-table>
  </v-card>
</template>

<script lang="ts" setup>
import { api } from '@/api';
import { mdiAlert, mdiApplicationOutline, mdiCheckCircleOutline, mdiChevronDown, mdiChevronUp, mdiCloseCircleOutline, mdiFileReplaceOutline, mdiMagnify, mdiOpenInNew, mdiPlay, mdiPlayBoxOutline, mdiPlayNetworkOutline, mdiRefresh } from '@mdi/js';
import type { PackageOptions, PackageResult, PackageSource, Project } from '@/packager';
import { onMounted, ref, type Ref } from 'vue';
import { useI18n } from 'vue-i18n';

const { t } = useI18n({ useScope: 'global' });

const busy: Ref<boolean> = ref(false);
const search = ref('')
const expanded: Ref<string[]> = ref([])
const packageSource: Ref<string> = ref("Default");
const packageSources: Ref<PackageSource[]> = ref([]);
const projects: Ref<Project[]> = ref([]);
const selected: Ref<string[]> = ref([]);
const packageOptions: Ref<PackageOptions> = ref({
  configuration: "Debug"
})

const headers: any[] = [
  {
    value: "action",
    headerProps: {
      class: "w-1"
    }
  },
  {
    headerProps: {
      class: "w-64"
    },
    align: 'end',
    title: t("version"),
    value: "version",
  },
  {
    title: t("name"),
    value: "name",
  }
];

const collapse = (id: string) => {
  const index = expanded.value.findIndex(item => item === id);

  if (index > -1) {
    expanded.value.splice(index, 1);
  }
}

const expand = (id: string) => {
  const index = expanded.value.findIndex(item => item === id);

  if (index === -1) {
    expanded.value.push(id);
  }
}

const toggle = (_: Event, { item }: { item: Project }) => {
  if (!item.version || !item.log) {
    collapse(item.id)
    return;
  }

  const index = expanded.value.findIndex(id => id === item.id);

  if (index === -1) {
    expanded.value.push(item.id);
  } else {
    expanded.value.splice(index, 1);
  }
}

const push = async (project?: Project) => {
  await execute("push", project)
}

const build = async (project?: Project) => {
  await execute("build", project)
}

const pack = async (project?: Project) => {
  await execute("pack", project)
}

const getProject = (id: string) => {
  const result = projects.value.find(item => item.id === id)

  if (!result) {
    throw new Error(`Could not find project with id '${id}'.`)
  }

  return result;
}

const execute = async (command: string, project?: Project) => {
  const items = project ? [project] : selected.value.map(id => getProject(id));

  items.forEach(async item => {
    item.busy = true

    try {
      item.log = ''

      collapse(item.id)

      const result = await api.patch<PackageResult>(`projects/${item.id}/${command}`, packageOptions.value)

      item.log = result.data.log;

      if (result.data.failed) {
        item.status = "failed"
        expand(item.id)
      } else {
        item.status = "ok"
      }

    } finally {
      item.busy = false;
    }
  });
}

const open = async (project: Project) => {
  await api.patch(`projects/${project.id}/open`)
}

const setVersion = async (project: Project) => {
  await api.patch(`projects/${project.id}/property`, {
    name: "version",
    value: project.vnext
  })

  project.version = project.vnext;
  project.editingVersion = false;
}

const openVersion = (project: Project) => {
  project.vnext = project.version;
  project.editingVersion = true;
}

const cancelVersion = (item: Project) => {
  item.editingVersion = false;
}

const fetchPackageSources = async () => {
  const response = await api.get("/package-sources");

  packageSources.value = response.data
}

const refresh = async () => {
  busy.value = true;

  try {
    const response = await api.get("/projects");

    projects.value = response.data.map((item: Project) => {
      item.selectable = !!item.version
      return item;
    })
  }
  finally {
    busy.value = false;
  }
}

const load = async () => {
  busy.value = true;

  try {
    await api.patch("/projects/load");
  }
  finally {
    busy.value = false;
  }

  await refresh()
  await fetchPackageSources()
}

onMounted(async () => {
  await load();
})
</script>
