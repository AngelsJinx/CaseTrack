import type { DateTime } from 'luxon';

export enum TaskStatus {
  pending,
  inProgress,
  onHold,
  completed,
}

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  payload: T;
}

export class Task {
  id: number;
  title: string;
  description?: string | undefined;
  status: TaskStatus;
  dueDate: DateTime;

  constructor(
    id: number,
    title: string,
    description: string | undefined,
    status: TaskStatus,
    dueDate: DateTime,
  ) {
    this.id = id;
    this.title = title;
    this.description = description;
    this.status = status;
    this.dueDate = dueDate;
  }
}
