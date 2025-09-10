import {defineStore} from "pinia";
import {ref} from "vue";
import type {Task, ApiResponse} from "components/models";
import {api} from "boot/axios";

export const useTaskStore = defineStore('task', () => {
  const tasks = ref<Task[]>()

  async function fetchTasks() {
    const response = await api.get<ApiResponse<Task[]>>('/task')
    tasks.value = response.data.payload
    // TODO error handling
  }

  return {
    tasks,
    fetchTasks,
  }
})
