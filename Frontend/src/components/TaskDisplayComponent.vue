<template>
  <q-card class="task-card" @mouseover="isHovering = true" @mouseleave="isHovering = false">
    <q-card-section horizontal>
      <q-card-section class="q-pb-sm col justify-between">
        <div class="text-h6">{{props.task.title}}</div>
        <div class="q-my-auto"><q-icon name="event" size="sm" class="q-mr-xs" />Due: {{props.task.dueDate.toFormat('yyyy-MM-dd')}}</div>
        <div class="description">
          <span v-if="props.task.description?.length">{{ props.task.description }}</span>
          <span v-else class="text-grey">This task has no description</span>
        </div>
      </q-card-section>
      <q-card-section v-show="isHovering" class="q-gutter-sm">
        <q-btn icon="edit" color="secondary" round class="row" @click="emits('edit', props.task)">
          <q-tooltip>Edit</q-tooltip>
        </q-btn>
        <q-btn icon="delete" color="negative" round class="row" @click="emits('delete', props.task)">
          <q-tooltip>Delete</q-tooltip>
        </q-btn>
      </q-card-section>
    </q-card-section>
  </q-card>
</template>
<script lang="ts" setup>
import type {Task} from "components/models";
import {ref} from "vue";

const props = defineProps<{ task: Task }>();
const emits = defineEmits<{
  (e: 'edit', task: Task): void,
  (e: 'delete', task: Task): void
}>()

const isHovering = ref<boolean>(false);
</script>
<style lang="scss" scoped>
.task-card {
  height: 13em;
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
