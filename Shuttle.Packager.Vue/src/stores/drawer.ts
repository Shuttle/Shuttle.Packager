import { defineStore } from "pinia";
import { ref } from "vue";

export const useDrawerStore = defineStore("drawer", () => {
  const filterDrawerVisible = ref(true);

  const toggleFilterDrawer = () => {
    filterDrawerVisible.value = !filterDrawerVisible.value;
  };

  return {
    filterDrawerVisible,
    toggleFilterDrawer,
  };
});
