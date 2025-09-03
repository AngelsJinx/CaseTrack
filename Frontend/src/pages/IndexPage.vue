<template>
  <q-page class="row justify-evenly no-wrap">
    <q-card v-for="column of columns" :key="column.status" class="q-mx-sm q-my-md col-xs-6 col-sm-4 col-md-3 col-lg task-list">
      <q-card-section class="text-h5">
        {{ column.name }}
      </q-card-section>
      <q-card-section class="bg-blue-grey-3 task-list">
        <task-display-component v-for="task of tasks.filter(t => t.status === column.status)" :key="task.id" :task="task" />
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script setup lang="ts">
import {ref} from 'vue';
import TaskDisplayComponent from "components/TaskDisplayComponent.vue";
import {Task, TaskStatus} from "components/models";
import {DateTime} from "luxon";

const tasks = ref<Task[]>([
  new Task(0, 'Dummy task', 'With a description', TaskStatus.pending, DateTime.now()),
  new Task(0, 'Do the things', 'Do great things, with great responsibilities. Impress folks. Be happy. Spread joy.', TaskStatus.inProgress, DateTime.now())
]);

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
  height: calc(100% - 55px);
}
</style>
