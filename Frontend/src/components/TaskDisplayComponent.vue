<template>
  <q-card class="task-card">
    <q-card-section class="q-pb-sm row justify-between">
      <div class="text-h6 col-lg col-xs-12">{{props.task.title}}</div>
      <div class="col-grow q-my-auto"><q-icon name="event" size="sm" class="q-mr-xs" />Due: {{props.task.dueDate.toLocaleString()}}</div>
    </q-card-section>
    <q-card-section class="q-pt-none">
      <div class="description">{{ props.task.description }}</div>
    </q-card-section>
  </q-card>
</template>
<script lang="ts" setup>
import type {Task} from "components/models";

const props = defineProps<{ task: Task }>();
</script>
<style lang="scss" scoped>
.task-card {
  min-height: 150px;
  overflow: hidden;

  // This fade-long-text-out solution copied from https://css-tricks.com/line-clampin/#aa-the-fade-out-way
  .description {
    overflow: hidden;
    position: relative;
    height: calc(1.5em * 3); // line-height * number of lines before fading
  }
  .description::after {
    content: "";
    text-align: right;
    position: absolute;
    bottom: 0;
    right: 0;
    width: 70%;
    height: 1.2em;
    background: linear-gradient(to right, rgba(255, 255, 255, 0), rgba(255, 255, 255, 1) 50%);
  }
}
</style>
