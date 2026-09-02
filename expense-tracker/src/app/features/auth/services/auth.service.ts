import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';

import { environment } from '../../../../environments/environment';

import {
  AuthResponse,
  AuthUser,
  LoginRequest,
  RegisterRequest,
} from '../../../core/models/auth.model';
import { CurrencyService } from '../../../core/services/CurrencyService.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly currencyService = inject(CurrencyService);

  private readonly apiUrl = `${environment.apiUrl}/Auth`;

  private readonly tokenKey = 'expense_tracker_token';
  private readonly userKey = 'expense_tracker_user';

  // Current logged-in user
  readonly currentUser = signal<AuthUser | null>(this.loadUser());

  // Login

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, request).pipe(
      tap((response) => {
        if (response.isSuccess && response.value?.token) {
          this.saveAuthData(response.value);
        }
      }),
    );
  }

  // Register

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.apiUrl}/register`, request)
      .pipe(
        tap((response) => {
          if (response.isSuccess && response.value?.token) {
            this.saveAuthData(response.value);
          }
        }),
      );
  }

  // Save Auth Data

  private saveAuthData(data: AuthResponse['value']): void {
    localStorage.setItem(this.tokenKey, data.token);

    const user: AuthUser = {
      userId: data.userId,
      name: data.name,
      email: data.email,
    };

    localStorage.setItem(this.userKey, JSON.stringify(user));

    // Update signal
    this.currentUser.set(user);
  }

  // Load User

  private loadUser(): AuthUser | null {
    const user = localStorage.getItem(this.userKey);

    if (!user) {
      return null;
    }

    try {
      return JSON.parse(user) as AuthUser;
    } catch {
      return null;
    }
  }

  // Get Token

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  // Get User

  getUser(): AuthUser | null {
    return this.currentUser();
  }

  // Update User

  updateCurrentUser(user: AuthUser): void {
    localStorage.setItem(this.userKey, JSON.stringify(user));

    this.currentUser.set(user);
  }

  // Authentication

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  // Logout

  logout(): void {
    localStorage.removeItem(this.tokenKey);

    localStorage.removeItem(this.userKey);

    this.currentUser.set(null);

    this.router.navigate(['/login']);
  }
}
