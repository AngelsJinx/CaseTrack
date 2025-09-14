import {defineStore} from "pinia";
import {ref} from "vue";
import {Task, TaskStatus, type ApiResponse} from "components/models";
import {api} from "boot/axios";
import { DateTime } from "luxon";

export const useTaskStore = defineStore('task', () => {
  const tasks = ref<Task[]>()

  async function fetchTasks() {
    const response = await api.get<ApiResponse<Task[]>>('/task')
    tasks.value = response.data.payload
    // TODO error handling
  }

  function initialiseNewTask(){
    return new Task(undefined, '', undefined, TaskStatus.pending, DateTime.now().plus({ days: 7 }))
  }

  return {
    tasks,
    fetchTasks,
    initialiseNewTask,
  }
})
