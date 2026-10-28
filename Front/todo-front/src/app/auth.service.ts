import { inject, Injectable, signal } from '@angular/core';
import { finalize, Observable, shareReplay, tap } from 'rxjs';
import { ApiService, AuthResult, User } from './api.service';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly api = inject(ApiService);
  private refreshRequest?: Observable<AuthResult>;
  readonly user = signal<User | null>(this.readUser());

  get token(): string | null { return localStorage.getItem('todo-token'); }
  get refreshToken(): string | null { return localStorage.getItem('todo-refresh-token'); }

  save(result: AuthResult): void {
    localStorage.setItem('todo-token', result.token);
    localStorage.setItem('todo-refresh-token', result.refreshToken);
    localStorage.setItem('todo-user', JSON.stringify(result.user));
    this.user.set(result.user);
  }

  refresh() {
    if (!this.refreshRequest) {
      this.refreshRequest = this.api.refresh(this.refreshToken ?? '').pipe(
        tap(result => this.save(result)),
        finalize(() => this.refreshRequest = undefined),
        shareReplay(1)
      );
    }
    return this.refreshRequest;
  }

  logout() {
    const refreshToken = this.refreshToken;
    if (!refreshToken) {
      this.clear();
      return;
    }
    return this.api.logout(refreshToken).pipe(finalize(() => this.clear()));
  }

  clear(): void {
    localStorage.removeItem('todo-token');
    localStorage.removeItem('todo-refresh-token');
    localStorage.removeItem('todo-user');
    this.user.set(null);
  }

  private readUser(): User | null {
    try {
      return JSON.parse(localStorage.getItem('todo-user') ?? 'null') as User | null;
    } catch {
      return null;
    }
  }
}
