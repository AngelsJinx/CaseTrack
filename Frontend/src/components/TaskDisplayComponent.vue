<template>
  <q-card class="task-card">
    <q-card-section class="text-h6 q-pb-sm">{{props.task.title}}</q-card-section>
    <q-card-section class="q-pt-none">
      <div>
        <span>
          Due:
        </span>
        <span>{{props.task.dueDate.toLocaleString()}}</span>
      </div>
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
