import { Component, inject, OnInit } from '@angular/core';

import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import { AuthService } from '../../../auth/services/auth.service';

import {
  UsersService,
  UpdateUserSettingsRequest,
} from '../../services/users.service';
import { CurrencyService } from '../../../../core/services/CurrencyService.service';

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.css',
})
export class SettingsComponent implements OnInit {
  private readonly fb = inject(FormBuilder);

  private readonly authService = inject(AuthService);
  private readonly currencyService = inject(CurrencyService);
  private readonly usersService = inject(UsersService);

  form: FormGroup = this.fb.group({
    name: ['', Validators.required],

    email: ['', [Validators.required, Validators.email]],

    currency: ['EGP', Validators.required],

    language: ['English', Validators.required],
  });

  isLoading = false;
  isSaving = false;

  successMessage = '';
  errorMessage = '';

  ngOnInit(): void {
    this.loadUser();
  }

  // GET USER

  loadUser(): void {
    this.isLoading = true;

    this.errorMessage = '';

    this.usersService.getMe().subscribe({
      next: (response) => {
        this.isLoading = false;

        if (!response.isSuccess || !response.value) {
          this.errorMessage = response.error ?? 'Failed to load settings.';

          return;
        }

        const user = response.value;

        this.form.patchValue({
          name: user.name,
          email: user.email,
          currency: user.currency,
          language: user.language,
        });
        this.currencyService.setCurrency(user.currency);
      },

      error: (error) => {
        this.isLoading = false;

        this.errorMessage =
          error?.error?.error ??
          error?.error?.message ??
          'Failed to load settings.';
      },
    });
  }

  // UPDATE USER

  saveSettings(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSaving = true;

    this.successMessage = '';
    this.errorMessage = '';

    const request: UpdateUserSettingsRequest = this.form.getRawValue();

    this.usersService.updateMe(request).subscribe({
      next: (response) => {
        this.isSaving = false;

        if (!response.isSuccess || !response.value) {
          this.errorMessage = response.error ?? 'Failed to save settings.';

          return;
        }

        const user = response.value;

        // Update AuthService
        this.authService.updateCurrentUser({
          userId: user.id,
          name: user.name,
          email: user.email,
        });
        this.currencyService.setCurrency(user.currency);
        this.successMessage = 'Settings saved successfully.';
      },

      error: (error) => {
        this.isSaving = false;

        this.errorMessage =
          error?.error?.error ??
          error?.error?.message ??
          'Failed to save settings.';
      },
    });
  }

  // Logout

  logout(): void {
    this.authService.logout();
  }
}
