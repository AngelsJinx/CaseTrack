<template>
  <q-page class="row justify-evenly no-wrap">
    <q-card v-for="column of columns" :key="column.status" class="q-mx-sm q-my-md col-xs-6 col-sm-4 col-md-3 col-lg task-list">
      <q-card-section class="text-h5 bg-white">
        {{ column.name }}
      </q-card-section>
      <q-card-section class="task-list q-gutter-y-md">
        <task-display-component
          v-for="task of taskStore.tasks?.filter(t => t.status === column.status)"
          :key="task.id!"
          :task="task"
          @edit="editTask"
          @delete="requestDelete"
        />
      </q-card-section>
    </q-card>
    <q-page-sticky position="bottom-right" :offset="[34,34]">
      <q-btn fab round color="secondary" @click="createNewTask">
        <q-icon name="add" size="lg" />
        <q-tooltip>Add a new Task</q-tooltip>
      </q-btn>
    </q-page-sticky>

    <TaskEditDialogComponent v-if="!!editingTask" v-model="showEditDialog" :task="editingTask" @save="onSave" />
  </q-page>
</template>

<script setup lang="ts">
import TaskDisplayComponent from "components/TaskDisplayComponent.vue";
import {TaskStatus, type Task} from "components/models";
import {useTaskStore} from "stores/taskStore";
import { ref } from "vue";
import TaskEditDialogComponent from "components/TaskEditDialogComponent.vue";
import {isAxiosError} from "axios";
import {useQuasar} from "quasar";

const $q = useQuasar();
const taskStore = useTaskStore();
const editingTask = ref<Task>();
const showEditDialog = ref(false);

function createNewTask() {
  editingTask.value = taskStore.initialiseNewTask();
  showEditDialog.value = true;
}

function editTask(task: Task) {
  editingTask.value = task;
  showEditDialog.value = true;
}

function requestDelete(task: Task){
  $q.dialog({
    title: 'Delete task?',
    message: 'It will be permanently deleted.',
    cancel: true,
    persistent: true,
    ok: {
      color: 'negative'
    }
  }).onOk(() => {
    deleteTask(task).catch(console.error); // Should do proper error recording
  })
}

async function deleteTask(task: Task){
  if (!task.id){
    console.error('Attempted to delete a task without an ID!')
    // Log this insane edge case!
  }
  try {
    await taskStore.deleteTask(task);
    $q.notify({
      message: 'Task deleted',
      icon: 'delete',
      color: 'positive',
    })
  } catch (error) {
    let errorMessage = "An error occurred while deleting the task. Please try again later.";
    if (isAxiosError(error) && error.response?.data?.message?.length){
      errorMessage = error.response.data.message;
    }
    $q.notify({
      message: errorMessage,
      color: 'negative'
    })
  }
}

async function onSave(updatedTask: Task) {
  try {
    const savedTask = await taskStore.saveTask(updatedTask);
    editingTask.value = savedTask;
    showEditDialog.value = false;

    $q.notify({
      message: 'Task saved',
      icon: 'save',
      color: 'positive',
    })
  } catch (error) {
    let errorMessage = "An error occurred while saving the task. Please try again later.";
    if (isAxiosError(error) && error.response?.data?.message?.length){
      errorMessage = error.response.data.message;
    }
    $q.notify({
      message: errorMessage,
      color: 'negative'
    })
  }
}

taskStore.fetchTasks()
  .catch(console.error); // Should use proper error logging!

interface TaskColumn {
  name: string;
  status: TaskStatus;
}

const columns: TaskColumn[] = [
  {
    name: 'Pending',
    status: TaskStatus.pending,
  },
  {
    name: 'On Hold',
    status: TaskStatus.onHold,
  },
  {
    name: 'In Progress',
    status: TaskStatus.inProgress,
  },
  {
    name: 'Completed',
    status: TaskStatus.completed,
  }
]
</script>
<style lang="scss" scoped>
.task-list {
  background-color: $light-grey;
}
</style>
