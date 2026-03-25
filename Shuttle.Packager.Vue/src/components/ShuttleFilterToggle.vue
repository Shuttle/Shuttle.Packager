<template>
  <v-btn :icon="drawerStore.filterDrawerVisible ? mdiFilterOffOutline : mdiFilterOutline" color="secondary"
    size="x-small" @click="drawerStore.toggleFilterDrawer"
    v-tooltip="drawerStore.filterDrawerVisible ? t('hide-filters') : t('show-filters')"></v-btn>
</template>

<script setup lang="ts">
import { mdiFilterOffOutline, mdiFilterOutline } from '@mdi/js'
import { useDrawerStore } from '@/stores/drawer'
import { useI18n } from 'vue-i18n'

const drawerStore = useDrawerStore()
const { t } = useI18n({ useScope: 'global' })
</script>
