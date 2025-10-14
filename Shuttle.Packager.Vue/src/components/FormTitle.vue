<template>
  <div class="flex flex-row align-middle sv-title items-start" :class="getClasses()">
    <div class="flex-grow">{{ props.title }}</div>
    <v-btn v-if="canClose" :icon="mdiClose" @click.stop="click" size="x-small" flat></v-btn>
  </div>
</template>

<script setup lang="ts">
import type { FormTitle } from '@/packager'
import { useRouter } from 'vue-router'
import { mdiClose } from '@mdi/js'

const router = useRouter()
const props = defineProps<FormTitle>()

const canClose = props.closePath || props.closeClick

const getClasses = () => {
  switch (props.type) {
    case "borderless": {
      return "sv-title--borderless"
    }
    default: {
      return "sv-title--normal"
    }
  }
}

const click = () => {
  if (props.closePath) {
    router.push(props.closePath)
    return
  }

  if (props.closeClick) {
    props.closeClick()
  }
}
</script>
