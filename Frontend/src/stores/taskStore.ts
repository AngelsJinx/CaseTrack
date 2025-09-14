import {defineStore} from "pinia";
import {ref} from "vue";
import {type ApiResponse, Task, TaskStatus} from "components/models";
import {api} from "boot/axios";
import {DateTime} from "luxon";

export const useTaskStore = defineStore('task', () => {
  const tasks = ref<Task[]>()

  async function fetchTasks() {
    const response = await api.get<ApiResponse<Task[]>>('/task')
    tasks.value = response.data.payload.map((task: Task) => {
      fixSerialization(task)
      return task
    })
    // TODO error handling
  }

  function initialiseNewTask(){
    return new Task(undefined, '', undefined, TaskStatus.pending, DateTime.now().plus({ days: 7 }))
  }

  async function saveTask(task: Task){
    const response = await api.post<ApiResponse<Task>>('/task', task)

    // Update the task
    const savedTask = response.data.payload
    fixSerialization(savedTask)
    const existingIndex = tasks.value?.findIndex(t => t.id === savedTask.id)
    if ((existingIndex ?? -1) >= 0) {
      tasks.value!.splice(existingIndex!, 1, savedTask)
    } else {
      tasks.value!.push(savedTask)
    }

    return savedTask
  }

  function fixSerialization(task: Task){
    // Axios deserialization is a bit of a nightmare, and gives us a string. It needs to be parsed into a luxon object.
    task.dueDate = DateTime.fromISO((task.dueDate as unknown) as string)
  }

  return {
    tasks,
    fetchTasks,
    initialiseNewTask,
    saveTask
  }
})
