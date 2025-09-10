<template>
  <q-page class="row justify-evenly no-wrap">
    <q-card v-for="column of columns" :key="column.status" class="q-mx-sm q-my-md col-xs-6 col-sm-4 col-md-3 col-lg task-list">
      <q-card-section class="text-h5 bg-white">
        {{ column.name }}
      </q-card-section>
      <q-card-section class="task-list">
        <task-display-component v-for="task of taskStore.tasks?.filter(t => t.status === column.status)" :key="task.id" :task="task" />
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script setup lang="ts">
import TaskDisplayComponent from "components/TaskDisplayComponent.vue";
import {TaskStatus} from "components/models";
import {useTaskStore} from "stores/taskStore";

const taskStore = useTaskStore();

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
  //height: calc(100% - 55px);
  background-color: $light-grey;
}
</style>
