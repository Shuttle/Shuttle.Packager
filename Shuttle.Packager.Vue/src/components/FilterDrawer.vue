<template>
  <v-navigation-drawer location="left" width="350">
    <form @submit.prevent="emit('filter')" class="p-4 flex flex-col gap-2">
      <slot></slot>
      <v-btn v-if="!props.hideFilter" :append-icon="mdiFilterOutline" type="submit" class="self-end">{{
        $t('filter')
      }}</v-btn>
    </form>
  </v-navigation-drawer>
</template>

<script setup lang="ts">
import { mdiFilterOutline } from '@mdi/js'

interface Props {
  hideFilter?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  hideFilter: false
})

const emit = defineEmits(['filter'])
</script>
