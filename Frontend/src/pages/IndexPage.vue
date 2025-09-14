<template>
  <q-page class="row justify-evenly no-wrap">
    <q-card v-for="column of columns" :key="column.status" class="q-mx-sm q-my-md col-xs-6 col-sm-4 col-md-3 col-lg task-list">
      <q-card-section class="text-h5 bg-white">
        {{ column.name }}
      </q-card-section>
      <q-card-section class="task-list">
        <task-display-component v-for="task of taskStore.tasks?.filter(t => t.status === column.status)" :key="task.id!" :task="task" />
      </q-card-section>
    </q-card>
    <q-page-sticky position="bottom-right" :offset="[18,18]">
      <q-btn fab icon="add" color="secondary" @click="createNewTask" />
    </q-page-sticky>

    <TaskEditDialogComponent v-if="!!editingTask" v-model="showEditDialog" :task="editingTask" @save="console.log" />
  </q-page>
</template>

<script setup lang="ts">
import TaskDisplayComponent from "components/TaskDisplayComponent.vue";
import {TaskStatus, type Task} from "components/models";
import {useTaskStore} from "stores/taskStore";
import { ref } from "vue";
import TaskEditDialogComponent from "components/TaskEditDialogComponent.vue";

const taskStore = useTaskStore();
const editingTask = ref<Task>();
const showEditDialog = ref(false);

function createNewTask() {
  editingTask.value = taskStore.initialiseNewTask();
  showEditDialog.value = true;
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
