import { Routes } from '@angular/router';

import { authGuard } from './core/guards/auth.guard';
import { guestGuard } from './core/guards/guestGuard';

export const routes: Routes = [
  // Authentication

  {
    path: '',
    canActivate: [guestGuard],

    loadComponent: () =>
      import('./layout/auth-layout/auth-layout/auth-layout.component').then(
        (m) => m.AuthLayoutComponent,
      ),

    children: [
      {
        path: '',
        redirectTo: 'login',
        pathMatch: 'full',
      },

      {
        path: 'login',
        loadComponent: () =>
          import('./features/auth/pages/login/login.component').then(
            (m) => m.LoginComponent,
          ),
      },

      {
        path: 'register',
        loadComponent: () =>
          import('./features/auth/pages/register/register.component').then(
            (m) => m.RegisterComponent,
          ),
      },
    ],
  },

  // Application

  {
    path: '',
    canActivate: [authGuard],

    loadComponent: () =>
      import('./layout/app-layout/app-layout.component').then(
        (m) => m.AppLayoutComponent,
      ),

    children: [
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./features/dashboard/pages/dashboard/dashboard.component').then(
            (m) => m.DashboardComponent,
          ),
      },

      {
        path: 'transactions',
        loadComponent: () =>
          import('./features/transactions/pages/transactions/transactions.component').then(
            (m) => m.TransactionsComponent,
          ),
      },

      {
        path: 'budgets',
        loadComponent: () =>
          import('./features/budgets/pages/budgets/budgets.component').then(
            (m) => m.BudgetsComponent,
          ),
      },

      {
        path: 'categories',
        loadComponent: () =>
          import('./features/categories/pages/categories/categories.component').then(
            (m) => m.CategoriesComponent,
          ),
      },

      {
        path: 'settings',
        loadComponent: () =>
          import('./features/settings/pages/settings/settings.component').then(
            (m) => m.SettingsComponent,
          ),
      },

      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full',
      },
    ],
  },

  // Fallback

  {
    path: '**',
    redirectTo: 'dashboard',
  },
];
