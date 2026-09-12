import { Component, computed, inject, signal } from '@angular/core';

import { DecimalPipe } from '@angular/common';

import { Budget, BudgetSummary } from '../../../../core/models/budget.model';

import { BudgetFormComponent } from '../../components/budget-form/budget-form.component';

import { BudgetsService } from '../../services/budgets.service';

import { ToastMessageService } from '../../../../core/services/toast-message.service';
import { CurrencyService } from '../../../../core/services/CurrencyService.service';

@Component({
  selector: 'app-budgets',
  standalone: true,
  imports: [DecimalPipe, BudgetFormComponent],
  templateUrl: './budgets.component.html',
  styleUrl: './budgets.component.css',
})
export class BudgetsComponent {
  private readonly budgetsService = inject(BudgetsService);
  protected readonly currencyService = inject(CurrencyService);
  private readonly toastMessage = inject(ToastMessageService);

  budgets = signal<Budget[]>([]);
  summaries = signal<BudgetSummary[]>([]);

  isLoading = signal(false);

  selectedBudget = signal<Budget | null>(null);

  isFormOpen = signal(false);

  ngOnInit(): void {
    this.loadBudgets();
    this.loadSummary();
  }

  loadBudgets(): void {
    this.isLoading.set(true);

    this.budgetsService.getAll().subscribe({
      next: (response) => {
        this.budgets.set(response.items);

        this.isLoading.set(false);
      },

      error: (error) => {
        this.isLoading.set(false);

        this.toastMessage.showMessage(
          error?.error?.message ?? 'Failed to load budgets.',
        );
      },
    });
  }

  loadSummary(): void {
    this.budgetsService.getSummary().subscribe({
      next: (response) => {
        this.summaries.set(response);
      },

      error: (error) => {
        this.toastMessage.showMessage(
          error?.error?.message ?? 'Failed to load budget summary.',
        );
      },
    });
  }

  // Getters

  totalBudget = computed(() =>
    this.summaries().reduce((total, budget) => total + budget.limit, 0),
  );

  totalSpent = computed(() =>
    this.summaries().reduce((total, budget) => total + budget.spent, 0),
  );

  remainingBudget = computed(() => this.totalBudget() - this.totalSpent());

  // =========================
  // Form
  // =========================

  openForm(): void {
    this.selectedBudget.set(null);

    this.isFormOpen.set(true);
  }

  closeForm(): void {
    this.isFormOpen.set(false);

    this.selectedBudget.set(null);
  }

  editBudget(summary: BudgetSummary): void {
    const budget: Budget = {
      id: summary.id,
      categoryId: summary.categoryId,
      categoryName: summary.category,
      limit: summary.limit,
    };

    this.selectedBudget.set(budget);
    this.isFormOpen.set(true);
  }

  // CREATE / UPDATE

  saveBudget(budget: Budget): void {
    const selected = this.selectedBudget();

    if (selected) {
      this.budgetsService
        .update({
          id: budget.id,
          categoryId: budget.categoryId,
          limit: budget.limit,
        })
        .subscribe({
          next: () => {
            this.toastMessage.showMessage('Budget updated successfully.');
            this.closeForm();
            this.loadBudgets();
            this.loadSummary();
          },
          error: (error) => {
            this.toastMessage.showMessage(
              error?.error?.message ?? 'Failed to update budget.',
            );
          },
        });

      return;
    }

    this.budgetsService
      .create({
        categoryId: budget.categoryId,
        limit: budget.limit,
      })
      .subscribe({
        next: () => {
          this.toastMessage.showMessage('Budget created successfully.');
          this.closeForm();
          this.loadBudgets();
          this.loadSummary();
        },
        error: (error) => {
          this.toastMessage.showMessage(
            error?.error?.message ?? 'Failed to create budget.',
          );
        },
      });
  }

  // DELETE

  deleteBudget(id: number): void {
    const confirmed = confirm('Are you sure you want to delete this budget?');

    if (!confirmed) {
      return;
    }

    this.budgetsService.delete(id).subscribe({
      next: () => {
        this.toastMessage.showMessage('Budget deleted successfully.');
        this.loadBudgets();
        this.loadSummary();
      },

      error: (error) => {
        this.toastMessage.showMessage(
          error?.error?.message ?? 'Failed to delete budget.',
        );
      },
    });
  }
}
