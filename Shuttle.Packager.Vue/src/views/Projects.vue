<template>
  <v-card flat>
    <v-card-title class="s-card-title">
      <s-title :title="$t('projects')" />
    </v-card-title>
    <div class="flex flex-wrap items-center gap-3 px-4 pb-3">
      <v-chip-group :model-value="preset" @update:model-value="applyPreset" mandatory selected-class="text-primary"
        class="py-0">
        <v-chip v-for="item in presets" :key="item.value" :value="item.value" :prepend-icon="item.icon" filter
          size="small" variant="outlined">
          {{ item.title }}
        </v-chip>
      </v-chip-group>
      <v-menu v-for="filter in visibleFilters" :key="filter.key" v-model="openFilterMenus[filter.key]"
        :close-on-content-click="false" location="bottom start">
        <template v-slot:activator="{ props: menuProps }">
          <v-chip v-bind="menuProps" :prepend-icon="filter.icon" color="primary" variant="tonal" size="small" closable
            @click:close="clearFilter(filter.key)">
            {{ describeFilter(filter.key) }}
          </v-chip>
        </template>
        <v-card min-width="320" class="p-3" :data-filter-editor="filter.key">
          <v-text-field v-if="filter.key === 'packageReference'" v-model="packageReferenceFilter"
            :label="t('package-reference')" :prepend-inner-icon="mdiMagnify" density="compact" variant="solo-filled"
            flat hide-details clearable @keydown.enter="openFilterMenus[filter.key] = false" />
        </v-card>
      </v-menu>
      <v-menu v-if="availableFilters.length > 0" location="bottom start">
        <template v-slot:activator="{ props: menuProps }">
          <v-btn v-bind="menuProps" :prepend-icon="mdiFilterPlusOutline" size="small" variant="outlined" rounded
            color="primary">
            {{ t("add-filter") }}
          </v-btn>
        </template>
        <v-list density="compact">
          <v-list-item v-for="filter in availableFilters" :key="filter.key" :prepend-icon="filter.icon"
            :title="filter.title" @click="addFilter(filter.key)" />
        </v-list>
      </v-menu>
      <v-btn v-if="hasNonDefaultFilters" :prepend-icon="mdiFilterRemoveOutline" size="small" variant="outlined" rounded
        color="secondary" @click="clearFilters">
        {{ t("clear-filters") }}
      </v-btn>
      <v-spacer />
      <v-menu :close-on-content-click="false" location="bottom end">
        <template v-slot:activator="{ props: menuProps }">
          <v-btn v-bind="menuProps" :icon="mdiCogOutline" size="small" variant="text"
            v-tooltip="t('options')"></v-btn>
        </template>
        <v-card min-width="320" class="p-3 flex flex-col gap-3">
          <v-switch v-model="allowPush" :label="t('allow-push')" color="primary" density="compact" hide-details />
          <v-select v-model="packageOptions.packageSourceName" :items="packageSources" item-title="name"
            item-value="name" :label="t('package-source')" density="compact" variant="solo-filled" flat clearable
            hide-details />
          <v-btn-toggle v-model="packageOptions.configuration" variant="outlined" density="compact" group mandatory
            class="w-full">
            <v-btn value="Debug" class="flex-1">
              {{ t("debug") }}
            </v-btn>
            <v-btn value="Release" class="flex-1">
              {{ t("release") }}
            </v-btn>
          </v-btn-toggle>
        </v-card>
      </v-menu>
      <v-btn :icon="mdiRefresh" size="small" variant="text" :loading="busy" @click="reload"
        v-tooltip="t('reload')"></v-btn>
    </div>
    <div class="flex flex-wrap items-center gap-3 px-4 pb-3">
      <div class="w-full sm:w-80">
        <v-text-field v-model="search" :label="t('find-in-results')" :prepend-inner-icon="mdiTextSearch"
          density="compact" variant="solo-filled" flat hide-details clearable />
      </div>
      <span class="text-medium-emphasis text-sm">{{ resultCount }}</span>
    </div>
    <v-divider></v-divider>
    <v-data-table :items="displayedItems" :headers="headers" v-model:expanded="expanded" @click:row="toggleExpanded"
      :mobile="null" density="default" mobile-breakpoint="md" :loading="busy" v-model="selected" show-select
      item-selectable="selectable" item-value="id" show-expand>
      <template v-slot:item.data-table-expand="{ internalItem, isExpanded, toggleExpand }">
        <v-btn v-if="!!internalItem.raw.log" :append-icon="isExpanded(internalItem) ? mdiChevronUp : mdiChevronDown"
          :text="isExpanded(internalItem) ? t('close') : t('show-log')" class="text-none"
          :class="internalItem.raw.status === 'failed' ? 'text-orange-400' : ''"
          :prepend-icon="internalItem.raw.status === 'failed' ? mdiAlert : undefined" size="small" slim
          @click.stop="toggleExpand(internalItem)"></v-btn>
      </template>
      <template v-slot:header.action="">
        <div class="s-strip my-2">
          <v-btn :icon="mdiPlay" size="x-small" @click="build()"></v-btn>
          <v-btn :icon="mdiPlayBoxOutline" size="x-small" @click="pack()"></v-btn>
          <v-btn v-if="allowPush" :icon="mdiUploadBoxOutline" size="x-small" @click="push()"></v-btn>
          <v-btn :icon="mdiHexadecimal" size="x-small" @click="getLatestVersion()"></v-btn>
        </div>
      </template>
      <template v-slot:item.action="{ item }">
        <v-speed-dial location="right center" transition="fade-transition" class="s-strip" open-on-hover>
          <template v-slot:activator="{ props: activatorProps }">
            <v-btn v-bind="activatorProps" :icon="mdiDotsHorizontalCircleOutline"></v-btn>
          </template>

          <div class="p-4 bg-neutral-700 border border-primary rounded-full gap-2 flex" :key="item.id">
            <v-btn v-if="item.selectable" :icon="mdiPlay" size="x-small" @click="build(item)"
              :disabled="item.busy"></v-btn>
            <v-btn v-if="item.selectable" :icon="mdiPlayBoxOutline" size="x-small" @click="pack(item)"
              :disabled="item.busy"></v-btn>
            <v-btn v-if="item.selectable && allowPush" :icon="mdiUploadBoxOutline" size="x-small" @click="push(item)"
              :disabled="item.busy"></v-btn>
            <v-btn v-if="item.selectable" :icon="mdiHexadecimal" size="x-small" @click="getLatestVersion(item)"
              :disabled="item.busy"></v-btn>
            <v-btn :icon="mdiApplicationOutline" size="x-small" @click="open(item)"></v-btn>
            <v-btn :icon="mdiOpenInNew" size="x-small" :href="`https://www.nuget.org/packages/${item.name}`"
              target="_blank"></v-btn>
            <v-btn :icon="mdiLink" size="x-small" @click="packageReferenceFilter = item.name"
              v-tooltip="t('filter-by-value', { value: item.name })"></v-btn>
          </div>
        </v-speed-dial>
      </template>
      <template v-slot:item.name="{ item }">
        <div class="flex flex-col my-2">
          <div class="flex flex-row gap-2">
            <div>{{ item.name }}</div>
            <v-icon :icon="getIcon(item)" @click.stop="togglePackageReferences(item)" class="text-neutral-500" />
          </div>
          <div v-if="item.showPackageReferences" class="flex flex-col gap-2 mt-2">
            <v-chip v-for="packageReference in item.packageReferences" :key="packageReference.name" density="compact"
              class="text-xs text-neutral-500" @click.stop="packageReferenceFilter = packageReference.name"
              v-tooltip="t('filter-by-value', { value: packageReference.name })">{{
                `${packageReference.name}@${packageReference.version}` }}</v-chip>
          </div>
        </div>
        <v-progress-linear v-if="item.busy" indeterminate />
      </template>
      <template v-slot:item.folder="{ item }">
        <span class="text-neutral-600 hover:text-neutral-300">{{ item.folder }}</span>
      </template>
      <template v-slot:item.latestVersion="{ item }">
        <div v-if="!!item.latestVersion" class="s-strip my-2 justify-end">
          <v-icon v-if="item.latestVersion !== item.version" :icon="mdiNotEqualVariant" class="text-orange-400" />
          <div :class="item.latestVersion !== item.version ? 'text-orange-400' : ''">{{ item.latestVersion }}</div>
        </div>
      </template>
      <template v-slot:item.version="{ item }">
        <form v-if="item.editingVersion" @submit.prevent="setVersion(item)" class="s-strip w-64 mt-2">
          <v-text-field v-model="item.vnext" hide-details variant="solo-filled" density="compact"
            @click.stop></v-text-field>
          <v-btn :icon="mdiCheckCircleOutline" size="x-small" @click.stop="setVersion(item)"></v-btn>
          <v-btn :icon="mdiCloseCircleOutline" size="x-small" @click.stop="cancelVersion(item)"></v-btn>
        </form>
        <span v-else class="cursor-pointer" @click.stop="openVersion(item)">{{ item.version }}</span>
      </template>
      <template v-slot:no-data>
        <div class="flex flex-col items-center gap-3 py-4">
          <span class="text-medium-emphasis">{{ t("no-results") }}</span>
          <v-btn v-if="hasNonDefaultFilters" :prepend-icon="mdiFilterRemoveOutline" size="small" variant="outlined"
            rounded color="secondary" @click="clearFilters">
            {{ t("clear-filters") }}
          </v-btn>
        </div>
      </template>
      <template #expanded-row="{ columns, item }">
        <tr>
          <td :colspan="columns.length">
            <div class="s-expand-container font-mono wrap-anywhere bg-neutral-900 text-neutral-300">
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
import {
  mdiAlert,
  mdiApplicationOutline,
  mdiCheckCircleOutline,
  mdiChevronDown,
  mdiChevronUp,
  mdiCloseCircleOutline,
  mdiCogOutline,
  mdiDotsHorizontalCircleOutline,
  mdiFilterPlusOutline,
  mdiFilterRemoveOutline,
  mdiHexadecimal,
  mdiInfinity,
  mdiLink,
  mdiLinkOff,
  mdiMagnify,
  mdiNotEqualVariant,
  mdiNumeric,
  mdiNumericOff,
  mdiOpenInNew,
  mdiPlay,
  mdiPlayBoxOutline,
  mdiRefresh,
  mdiTextSearch,
  mdiUploadBoxOutline
} from '@mdi/js';
import type { PackageOptions, PackageResult, PackageSource, PackageVersion, Project } from '@/packager';
import { computed, nextTick, onMounted, ref, type Ref } from 'vue';
import { useI18n } from 'vue-i18n';

const { t } = useI18n({ useScope: 'global' });

type ProjectType = "versioned" | "unversioned" | "all";
type FilterKey = "packageReference";

const defaultProjectType: ProjectType = "versioned";

const busy: Ref<boolean> = ref(false);
const search: Ref<string | null> = ref('')
const packageReferenceFilter: Ref<string | null> = ref('')
const expanded: Ref<string[]> = ref([])
const allowPush: Ref<boolean> = ref(false)
const projectType: Ref<ProjectType> = ref(defaultProjectType)
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
    cellProps: {
      class: "whitespace-nowrap"
    },
    align: 'end',
    title: t("version"),
    value: "version",
  },
  {
    title: t("name"),
    value: "name",
  },
  {
    title: t("folder"),
    value: "folder"
  },
  {
    cellProps: {
      class: "whitespace-nowrap"
    },
    align: 'end',
    title: t("latest-version"),
    value: "latestVersion",
  },
];

const presets = computed(() => [
  { value: "versioned", title: t("versioned"), icon: mdiNumeric },
  { value: "unversioned", title: t("unversioned"), icon: mdiNumericOff },
  { value: "all", title: t("all"), icon: mdiInfinity },
]);

const preset = computed(() => projectType.value);

const applyPreset = (value: unknown) => {
  if (value === "versioned" || value === "unversioned" || value === "all") {
    projectType.value = value;
  }
}

const filterDefinitions = computed(() => [
  { key: "packageReference" as FilterKey, title: t("package-reference"), icon: mdiLink },
]);

const addedFilters: Ref<FilterKey[]> = ref([]);
const openFilterMenus: Ref<Partial<Record<FilterKey, boolean>>> = ref({});

const hasValue = (key: FilterKey) => {
  switch (key) {
    case "packageReference":
      return !!packageReferenceFilter.value;
  }
}

const visibleFilters = computed(() =>
  filterDefinitions.value.filter(filter => hasValue(filter.key) || addedFilters.value.includes(filter.key)));

const availableFilters = computed(() =>
  filterDefinitions.value.filter(filter => !visibleFilters.value.some(visible => visible.key === filter.key)));

const describeFilter = (key: FilterKey) => {
  const definition = filterDefinitions.value.find(filter => filter.key === key);

  switch (key) {
    case "packageReference":
      return packageReferenceFilter.value ? `${definition?.title}: ${packageReferenceFilter.value}` : definition?.title;
  }
}

const addFilter = async (key: FilterKey) => {
  if (!addedFilters.value.includes(key)) {
    addedFilters.value.push(key);
  }

  await nextTick();

  openFilterMenus.value[key] = true;

  // the editor is only focusable once the menu content has rendered and the "Add filter" menu has closed
  setTimeout(() => document.querySelector<HTMLInputElement>(`[data-filter-editor="${key}"] input`)?.focus(), 250);
}

const clearFilter = (key: FilterKey) => {
  switch (key) {
    case "packageReference":
      packageReferenceFilter.value = '';
      break;
  }

  addedFilters.value = addedFilters.value.filter(item => item !== key);
  openFilterMenus.value[key] = false;
}

const hasNonDefaultFilters = computed(() =>
  projectType.value !== defaultProjectType || filterDefinitions.value.some(filter => hasValue(filter.key)));

const clearFilters = () => {
  projectType.value = defaultProjectType;
  filterDefinitions.value.forEach(filter => clearFilter(filter.key));
}

const filteredProjects = computed(() => {
  const packageReferenceMatch = (packageReferenceFilter.value ?? '').toLowerCase();

  let result = projects.value.filter(project =>
    (projectType.value === "versioned" && !!project.version) ||
    (projectType.value === "unversioned" && !project.version) ||
    (projectType.value === "all")
  );

  if (packageReferenceMatch) {
    result = result.filter(project => (project.packageReferences ?? []).some(reference => reference.name.toLowerCase().includes(packageReferenceMatch)))

    result.forEach(project => project.showPackageReferences = true);
  }

  return result;
})

const matchesSearch = (project: Project, match: string) =>
  headers
    .filter(header => typeof header.value === "string" && !!header.title)
    .some(header => String((project as Record<string, unknown>)[header.value] ?? '').toLowerCase().includes(match));

const displayedItems = computed(() => {
  const match = (search.value ?? '').trim().toLowerCase();

  return match
    ? filteredProjects.value.filter(project => matchesSearch(project, match))
    : filteredProjects.value;
})

const resultCount = computed(() => {
  const total = filteredProjects.value.length;
  const count = displayedItems.value.length;

  return count === total
    ? t("result-count", { count: total }, total)
    : t("result-count-of", { count, total });
})

const getIcon = (project: Project) => {
  return project.showPackageReferences ? mdiLinkOff : mdiLink;
}

const togglePackageReferences = (project: Project) => {
  project.showPackageReferences = !project.showPackageReferences;
}

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

const toggleExpanded = (_: Event, { item }: { item: Project }) => {
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

const getLatestVersion = (project?: Project) => {
  const items = project ? [project] : selected.value.map(id => getProject(id));

  items.forEach(async item => {
    item.busy = true

    try {
      item.log = ''

      collapse(item.id)

      const result = await api.get<PackageVersion>(`projects/${item.id}/package-version`, {
        params: { packageSourceName: packageOptions.value.packageSourceName }
      })

      item.latestVersion = result.data.version
    } finally {
      item.busy = false;
    }
  });
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

const reload = async () => {
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
  await reload();
})
</script>
