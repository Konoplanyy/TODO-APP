import { ChangeDetectorRef, Component, inject, OnDestroy, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { Subscription } from 'rxjs';
import { CategoryBtn } from './category-btn/category-btn';
import { SearchField } from './search-field/search-field';
import { SelectCategories } from './select-categories/select-categories';
import { TaskPreview } from './task-preview/task-preview';
import { PaginationBtn } from './pagination-btn/pagination-btn';
import { ColorPicker } from './color-picker/color-picker';
import { ApiService } from './api.service';
import { AuthService } from './auth.service';
import { Category, CategoryInput } from '../models/Category.model';
import { Task, TaskInput } from '../models/Task.model';

@Component({
  imports: [FormsModule, CategoryBtn, SearchField, SelectCategories, TaskPreview, PaginationBtn, ColorPicker],
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App implements OnInit, OnDestroy {
  private readonly api = inject(ApiService);
  private readonly changeDetector = inject(ChangeDetectorRef);
  readonly auth = inject(AuthService);
  private tasksRequest?: Subscription;
  private searchTimer?: ReturnType<typeof setTimeout>;

  categories: Category[] = [];
  tasks: Task[] = [];
  selectedPage = 1;
  readonly pageSize = 10;
  totalCount = 0;
  search = '';
  categoryId = '';
  loading = false;
  errorMessage = '';

  authMode: 'login' | 'register' = 'login';
  email = '';
  password = '';
  username = '';
  authBusy = false;

  taskEditorOpen = false;
  editingTaskId: string | null = null;
  taskForm: TaskInput = this.emptyTask();
  categoryEditorOpen = false;
  editingCategoryId: string | null = null;
  categoryForm: CategoryInput = { name: '', color: '#4F8EF7' };
  saving = false;

  get totalPages(): number {
    return Math.max(1, Math.ceil(this.totalCount / this.pageSize));
  }

  get visiblePages(): number[] {
    const start = Math.max(1, Math.min(this.selectedPage - 2, this.totalPages - 4));
    return Array.from({ length: Math.min(5, this.totalPages - start + 1) }, (_, index) => start + index);
  }

  ngOnInit(): void {
    if (this.auth.refreshToken) {
      this.auth.refresh().subscribe({
        next: () => this.loadData(),
        error: () => this.auth.clear(),
      });
    } else {
      this.auth.clear();
    }
  }

  ngOnDestroy(): void {
    this.tasksRequest?.unsubscribe();
    if (this.searchTimer) clearTimeout(this.searchTimer);
  }

  submitAuth(): void {
    this.errorMessage = '';
    this.authBusy = true;

    if (this.authMode === 'register') {
      this.api.register(this.username.trim(), this.email.trim(), this.password).subscribe({
        next: () => this.login(),
        error: error => { this.authBusy = false; this.showError(error); },
      });
    } else {
      this.login();
    }
  }

  private login(): void {
    this.api.login(this.email.trim(), this.password).subscribe({
      next: result => {
        this.auth.save(result);
        this.authBusy = false;
        this.password = '';
        this.loadData();
        this.changeDetector.markForCheck();
      },
      error: error => { this.authBusy = false; this.showError(error); },
    });
  }

  logout(): void {
    const result = this.auth.logout();
    if (result) result.subscribe({ error: () => this.auth.clear() });
    this.tasksRequest?.unsubscribe();
    this.categories = [];
    this.tasks = [];
    this.totalCount = 0;
    this.categoryId = '';
    this.errorMessage = '';
  }

  loadData(): void {
    this.loadCategories();
    this.loadTasks();
  }

  loadCategories(): void {
    this.api.getCategories().subscribe({
      next: categories => {
        this.categories = categories;
        this.changeDetector.markForCheck();
      },
      error: error => this.showError(error),
    });
  }

  loadTasks(): void {
    this.tasksRequest?.unsubscribe();
    this.loading = true;
    this.changeDetector.markForCheck();
    this.tasksRequest = this.api.getTasks(this.selectedPage, this.pageSize, this.search, this.categoryId).subscribe({
      next: result => {
        this.tasks = result.items;
        this.totalCount = result.totalCount;
        this.loading = false;
        if (this.selectedPage > this.totalPages) this.onPageChange(this.totalPages);
        this.changeDetector.markForCheck();
      },
      error: error => { this.loading = false; this.showError(error); },
    });
  }

  onSearchChange(value: string): void {
    this.search = value;
    this.selectedPage = 1;
    if (this.searchTimer) clearTimeout(this.searchTimer);
    this.searchTimer = setTimeout(() => this.loadTasks(), 300);
  }

  onCategoryChange(id: string): void {
    this.categoryId = id;
    this.selectedPage = 1;
    this.loadTasks();
  }

  onPageChange(page: number): void {
    if (page < 1 || page > this.totalPages || page === this.selectedPage) return;
    this.selectedPage = page;
    this.loadTasks();
  }

  categoryFor(task: Task): Category | undefined {
    return this.categories.find(category => category.id === task.categoryId);
  }

  openTaskEditor(task?: Task): void {
    this.editingTaskId = task?.id ?? null;
    this.taskForm = task ? {
      name: task.name,
      description: task.description,
      dueDate: this.toLocalDate(task.dueDate),
      isCompleted: task.isCompleted,
      categoryId: task.categoryId,
    } : this.emptyTask();
    this.taskEditorOpen = true;
    this.errorMessage = '';
  }

  saveTask(): void {
    if (!this.taskForm.name.trim() || !this.taskForm.categoryId || !this.taskForm.dueDate) return;
    const task: TaskInput = {
      ...this.taskForm,
      name: this.taskForm.name.trim(),
      dueDate: this.taskForm.dueDate,
    };
    this.saving = true;
    const request = this.editingTaskId
      ? this.api.updateTask(this.editingTaskId, task)
      : this.api.createTask(task);
    request.subscribe({
      next: () => {
        this.saving = false;
        this.taskEditorOpen = false;
        this.loadTasks();
        this.changeDetector.markForCheck();
      },
      error: error => { this.saving = false; this.showError(error); },
    });
  }

  onCompletedChange(task: Task): void {
    this.api.updateTask(task.id, this.taskInput(task)).subscribe({
      next: () => this.loadTasks(),
      error: error => this.showError(error),
    });
  }

  deleteTask(task: Task): void {
    if (!window.confirm(`Delete "${task.name}"?`)) return;
    this.api.deleteTask(task.id).subscribe({
      next: () => this.loadTasks(),
      error: error => this.showError(error),
    });
  }

  openCategoryEditor(category?: Category): void {
    this.editingCategoryId = category?.id ?? null;
    this.categoryForm = category ? { name: category.name, color: category.color } : { name: '', color: '#4F8EF7' };
    this.categoryEditorOpen = true;
    this.errorMessage = '';
  }

  saveCategory(): void {
    if (!this.categoryForm.name.trim()) return;
    this.saving = true;
    const category = { ...this.categoryForm, name: this.categoryForm.name.trim() };
    const request = this.editingCategoryId
      ? this.api.updateCategory(this.editingCategoryId, category)
      : this.api.createCategory(category);
    request.subscribe({
      next: saved => {
        this.saving = false;
        this.categoryEditorOpen = false;
        this.loadCategories();
        if (!this.editingCategoryId) this.categoryId = saved.id;
        this.loadTasks();
        this.changeDetector.markForCheck();
      },
      error: error => { this.saving = false; this.showError(error); },
    });
  }

  deleteCategory(category: Category): void {
    if (!window.confirm(`Delete category "${category.name}"?`)) return;
    this.api.deleteCategory(category.id).subscribe({
      next: () => {
        if (this.categoryId === category.id) this.categoryId = '';
        this.loadCategories();
        this.loadTasks();
      },
      error: error => this.showError(error),
    });
  }

  private emptyTask(): TaskInput {
    return {
      name: '', description: '', dueDate: this.toLocalDate(new Date().toISOString()),
      isCompleted: false, categoryId: this.categories[0]?.id ?? '',
    };
  }

  private toLocalDate(value: string): string {
    const date = new Date(value);
    const local = new Date(date.getTime() - date.getTimezoneOffset() * 60000);
    return local.toISOString().slice(0, 16);
  }

  private taskInput(task: Task): TaskInput {
    return {
      name: task.name, description: task.description, dueDate: task.dueDate,
      isCompleted: task.isCompleted, categoryId: task.categoryId,
    };
  }

  private showError(error: HttpErrorResponse): void {
    this.errorMessage = error.error?.message ?? error.error?.title ?? 'Something went wrong. Please try again.';
    this.changeDetector.markForCheck();
  }
}
