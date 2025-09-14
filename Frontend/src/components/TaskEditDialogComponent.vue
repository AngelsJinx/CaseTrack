<template>
  <q-dialog v-model="model" persistent>
    <q-card>
      <q-card-section class="text-h6">
        {{ workingTask.id ? 'Edit Task' : 'Add Task' }}
      </q-card-section>
      <q-card-section class="q-mx-xs">
        <q-form ref="taskForm" class="q-gutter-sm">
          <q-input
            v-model="workingTask.title"
            filled
            label="Title"
            :rules="[val => !!val || 'Title is required']"
            @change="userMadeChanges = true" />
          <q-select
            v-model="workingTask.status"
            filled
            label="Status"
            :options="statusOptions"
            option-value="value"
            option-label="name"
            option-disable="disabled"
            :emit-value="true"
            :map-options="true"
            @update:model-value="userMadeChanges = true" />
          <q-date
            v-model="date"
            class="full-width"
            :options="isValidDueDate"
            @update:model-value="userMadeChanges = true" />
          <q-input v-model="workingTask.description" filled label="Description" type="textarea" @change="userMadeChanges = true" />
        </q-form>
      </q-card-section>
      <q-card-actions class="justify-end">
        <q-btn icon="close" label="Cancel" @click="discardChangesPrompt" />
        <q-btn icon="save" label="Save" color="secondary" @click="saveChanges" />
      </q-card-actions>
    </q-card>
  </q-dialog>
</template>
<script setup lang="ts">
import {type Task, TaskStatus} from "components/models";
import {computed, ref, toRaw, watch} from "vue";
import { cloneDeep } from 'lodash';
import { useQuasar } from 'quasar';
import { DateTime } from 'luxon';


const $q = useQuasar();
const taskForm = ref();

const model = defineModel<boolean>({ required: true })
const props = defineProps<{ task: Task }>()
const emits = defineEmits<{
  (e: 'save', task: Task): void
}>()

const userMadeChanges = ref(false)
const workingTask = ref<Task>(cloneDeep(toRaw(props.task)))
watch(
  () => props.task,
  (taskModel: Task) => {
    cloneTask(taskModel)
  },
);
const date = ref<string>(props.task.dueDate.toFormat('YYYY/MM/DD'));

function cloneTask(task: Task) {
  workingTask.value = cloneDeep(toRaw(task))
  userMadeChanges.value = false;
  date.value = workingTask.value.dueDate.toFormat('yyyy/MM/dd')
}

const statusOptions = computed(() => [{
    name: 'Pending',
    value: TaskStatus.pending,
    disabled: false
  },
  {
    name: 'On Hold',
    value: TaskStatus.onHold,
    disabled: false
  },
  {
    name: 'In Progress',
    value: TaskStatus.inProgress,
    disabled: false
  },
  {
    name: 'Completed',
    value: TaskStatus.completed,
    disabled: !workingTask.value.id
  }]
)

function discardChangesPrompt() {
  if (userMadeChanges.value) {
    $q.dialog({
      title: 'Discard changes?',
      message: 'Your changes will be lost.',
      cancel: true,
      persistent: true,
      ok: {
        color: 'negative'
      }
    }).onOk(() => {
      model.value = false
    })
  } else {
    model.value = false
  }
  taskForm.value.resetValidation()
}

async function saveChanges() {
  if (await taskForm.value.validate()) {
    // Unfortunately q-date works only in strings, so make sure we have a properly-typed datetime before returning
    workingTask.value.dueDate = DateTime.fromFormat(date.value, 'yyyy/MM/dd');
    emits('save', workingTask.value)
  }
}

function isValidDueDate(date: string): boolean {
  const parsedDate = new Date(date);
  parsedDate.setHours(0, 0, 0, 0);
  const currentDate = new Date();
  currentDate.setHours(0, 0, 0, 0);
  return  parsedDate >= currentDate;
}


cloneTask(props.task);
</script>
<style scoped lang="scss">

</style>
