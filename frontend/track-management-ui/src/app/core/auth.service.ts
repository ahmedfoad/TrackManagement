import { computed, inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { LoginResponse } from './track.models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly storedToken = signal(localStorage.getItem('track-management-token'));
  readonly username = signal(localStorage.getItem('track-management-username') ?? '');
  readonly isAuthenticated = computed(() => Boolean(this.storedToken()));

  login(email: string, password: string) {
    return this.http
      .post<LoginResponse>('/api/auth/login', { email, password })
      .pipe(tap((response) => this.saveSession(response)));
  }

  logout(): void {
    localStorage.removeItem('track-management-token');
    localStorage.removeItem('track-management-username');
    this.storedToken.set(null);
    this.username.set('');
  }

  token(): string | null {
    return this.storedToken();
  }

  private saveSession(response: LoginResponse): void {
    localStorage.setItem('track-management-token', response.token);
    localStorage.setItem('track-management-username', response.username);
    this.storedToken.set(response.token);
    this.username.set(response.username);
  }
}
