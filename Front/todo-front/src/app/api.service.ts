import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Category, CategoryInput } from '../models/Category.model';
import { PagedResult, Task, TaskInput } from '../models/Task.model';

export type User = { id: string; username: string; email: string };
export type AuthResult = { token: string; refreshToken: string; user: User };

@Injectable({ providedIn: 'root' })
export class ApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api';

  login(email: string, password: string) {
    return this.http.post<AuthResult>(`${this.baseUrl}/Users/login`, { email, password });
  }

  register(username: string, email: string, password: string) {
    return this.http.post<User>(`${this.baseUrl}/Users/register`, { username, email, password });
  }

  refresh(refreshToken: string) {
    return this.http.post<AuthResult>(`${this.baseUrl}/Users/refresh`, { refreshToken });
  }

  logout(refreshToken: string) {
    return this.http.post<void>(`${this.baseUrl}/Users/logout`, { refreshToken });
  }

  getCategories() {
    return this.http.get<Category[]>(`${this.baseUrl}/Categories`);
  }

  createCategory(category: CategoryInput) {
    return this.http.post<Category>(`${this.baseUrl}/Categories`, category);
  }

  updateCategory(id: string, category: CategoryInput) {
    return this.http.put<Category>(`${this.baseUrl}/Categories/${id}`, category);
  }

  deleteCategory(id: string) {
    return this.http.delete<void>(`${this.baseUrl}/Categories/${id}`);
  }

  getTasks(page: number, pageSize: number, search: string, categoryId: string) {
    let params = new HttpParams().set('page', page).set('pageSize', pageSize);
    if (search.trim()) params = params.set('search', search.trim());
    if (categoryId) params = params.set('categoryId', categoryId);
    return this.http.get<PagedResult<Task>>(`${this.baseUrl}/TaskItems`, { params });
  }

  createTask(task: TaskInput) {
    return this.http.post<Task>(`${this.baseUrl}/TaskItems`, task);
  }

  updateTask(id: string, task: TaskInput) {
    return this.http.put<Task>(`${this.baseUrl}/TaskItems/${id}`, task);
  }

  deleteTask(id: string) {
    return this.http.delete<void>(`${this.baseUrl}/TaskItems/${id}`);
  }
}
