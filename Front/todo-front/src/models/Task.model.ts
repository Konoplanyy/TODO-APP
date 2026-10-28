export type Task = {
  id: string;
  name: string;
  description: string;
  dueDate: string;
  isCompleted: boolean;
  categoryId: string;
  createdAt: string;
  lastUpdatedAt: string;
};

export type TaskInput = Pick<Task, 'name' | 'description' | 'dueDate' | 'isCompleted' | 'categoryId'>;

export type PagedResult<T> = {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
};
