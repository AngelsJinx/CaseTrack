<template>
  <q-dialog v-model="model" persistent>
    <q-card>
      <q-card-section class="text-h6">
        {{ workingTask.id ? 'Edit Task' : 'Add Task' }}
      </q-card-section>
      <q-card-section>
        <q-form class="q-gutter-sm">
          <q-input v-model="workingTask.title" filled label="Title" @change="userMadeChanges = true" />
          <q-select
            v-model="workingTask.status"
            filled
            label="Status"
            :options="statusOptions"
            option-value="value"
            option-label="name"
            :emit-value="true"
            :map-options="true"
            @change="userMadeChanges = true" />
          <q-date v-model="date" @change="userMadeChanges = true" class="full-width" />
          <q-input v-model="workingTask.description" filled label="Description" type="textarea" @change="userMadeChanges = true" />
        </q-form>
      </q-card-section>
      <q-card-actions class="justify-end">
        <q-btn icon="close" label="Cancel" color="negative" @click="discardChangesPrompt" />
        <q-btn icon="save" label="Save" color="secondary" @click="saveChanges" />
      </q-card-actions>
    </q-card>
  </q-dialog>
</template>
<script setup lang="ts">
import {type Task, TaskStatus} from "components/models";
import {ref, toRaw, watch} from "vue";
import { cloneDeep } from 'lodash';
import { useQuasar } from 'quasar';


const $q = useQuasar();


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
  console.log(workingTask.value.dueDate.toFormat('yyyy/MM/dd'))
  date.value = workingTask.value.dueDate.toFormat('yyyy/MM/dd')
}

const statusOptions = [{
  name: 'Pending',
  value: TaskStatus.pending
},
  {
    name: 'On Hold',
    value: TaskStatus.onHold
  },
  {
    name: 'In Progress',
    value: TaskStatus.inProgress
  },
  {
    name: 'Completed',
    value: TaskStatus.completed
  }]

function discardChangesPrompt() {
  $q.dialog({
    title: 'Discard changes?',
    message: 'Your changes will be lost.',
    cancel: true,
    persistent: true,
    ok: {
      color: 'negative'
    }
  }).onOk(() => {
    model.value = false;
  })
}

function saveChanges() {
  emits('save', workingTask.value)
}


cloneTask(props.task);
</script>
<style scoped lang="scss">

</style>
