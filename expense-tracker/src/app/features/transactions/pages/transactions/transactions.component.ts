import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';

import { Transaction } from '../../../../core/models/transaction.module';
import { TransactionFormComponent } from '../../components/transaction-form/transaction-form.component';
import { ToastMessageService } from '../../../../core/services/toast-message.service';
import { TransactionsService } from '../../services/transactions.service';
import {
  Category,
  TransactionType,
} from '../../../../core/models/category.model';
import { CategoriesService } from '../../../categories/services/categories.service';
import { CurrencyService } from '../../../../core/services/CurrencyService.service';

@Component({
  selector: 'app-transactions',
  standalone: true,
  imports: [CommonModule, TransactionFormComponent],
  templateUrl: './transactions.component.html',
  styleUrl: './transactions.component.css',
})
export class TransactionsComponent {
  private readonly transactionsService = inject(TransactionsService);
  private readonly categoriesService = inject(CategoriesService);
  protected readonly currencyService = inject(CurrencyService);
  private readonly toastMessage = inject(ToastMessageService);

  // State

  transactions = signal<Transaction[]>([]);

  categories = signal<Category[]>([]);

  isLoading = signal(false);

  pageNumber = signal(1);

  pageSize = 10;

  totalCount = signal(0);

  totalPages = signal(0);

  selectedTransaction = signal<Transaction | null>(null);

  isFormOpen = signal(false);

  // Filters

  searchTerm = signal('');

  selectedType = signal<'all' | 'income' | 'expense'>('all');

  selectedCategory = signal<number | 'all'>('all');
  // Lifecycle

  ngOnInit(): void {
    this.loadTransactions();
    this.loadCategories();
  }

  // Load

  private getTransactionType(): TransactionType | undefined {
    const type = this.selectedType();

    if (type === 'all') {
      return undefined;
    }

    return type === 'income' ? TransactionType.Income : TransactionType.Expense;
  }

  loadTransactions(): void {
    this.isLoading.set(true);

    const category =
      this.selectedCategory() === 'all'
        ? undefined
        : (this.selectedCategory() as number);

    const type = this.getTransactionType();

    this.transactionsService
      .getAll(
        this.pageNumber(),
        this.pageSize,
        this.searchTerm().trim() || undefined,
        type,
        category,
      )
      .subscribe({
        next: (response) => {
          this.transactions.set(response.items);

          this.totalCount.set(response.totalCount);

          this.totalPages.set(response.totalPages);

          this.isLoading.set(false);
        },

        error: (error) => {
          this.isLoading.set(false);

          this.toastMessage.showMessage(
            error?.error?.message ?? 'Failed to load transactions.',
          );
        },
      });
  }

  loadCategories(): void {
    this.categoriesService.getAll().subscribe({
      next: (response) => {
        this.categories.set(response.items);
      },

      error: (error) => {
        this.toastMessage.showMessage(
          error?.error?.message ?? 'Failed to load categories.',
        );
      },
    });
  }

  // Pagination

  nextPage(): void {
    if (this.pageNumber() >= this.totalPages()) {
      return;
    }

    this.pageNumber.update((page) => page + 1);

    this.loadTransactions();
  }

  previousPage(): void {
    if (this.pageNumber() <= 1) {
      return;
    }

    this.pageNumber.update((page) => page - 1);

    this.loadTransactions();
  }

  // Form

  openForm(): void {
    this.selectedTransaction.set(null);

    this.isFormOpen.set(true);
  }

  closeForm(): void {
    this.isFormOpen.set(false);

    this.selectedTransaction.set(null);
  }

  editTransaction(transaction: Transaction): void {
    this.selectedTransaction.set(transaction);
    this.isFormOpen.set(true);
  }

  // Create / Update

  saveTransaction(transaction: Transaction): void {
    const selected = this.selectedTransaction();

    if (selected) {
      this.transactionsService
        .update({
          id: transaction.id,
          title: transaction.title,
          description: transaction.description,
          amount: transaction.amount,
          type: transaction.type,
          date: transaction.date,
          categoryId: transaction.categoryId,
        })
        .subscribe({
          next: () => {
            this.toastMessage.showMessage('Transaction updated successfully.');
            this.closeForm();
            this.loadTransactions();
          },
          error: (error) => {
            this.toastMessage.showMessage(
              error?.error?.message ?? 'Failed to update transaction.',
            );
          },
        });

      return;
    }

    this.transactionsService
      .create({
        title: transaction.title,
        description: transaction.description,
        amount: transaction.amount,
        type: transaction.type,
        date: transaction.date,
        categoryId: transaction.categoryId,
      })
      .subscribe({
        next: () => {
          this.toastMessage.showMessage('Transaction added successfully.');
          this.closeForm();
          this.pageNumber.set(1);
          this.loadTransactions();
        },
        error: (error) => {
          this.toastMessage.showMessage(
            error?.error?.message ?? 'Failed to add transaction.',
          );
        },
      });
  }

  // Delete

  deleteTransaction(id: number): void {
    const confirmed = confirm(
      'Are you sure you want to delete this transaction?',
    );

    if (!confirmed) {
      return;
    }

    this.transactionsService.delete(id).subscribe({
      next: () => {
        this.toastMessage.showMessage('Transaction deleted successfully.');

        this.loadTransactions();
      },

      error: (error) => {
        this.toastMessage.showMessage(
          error?.error?.message ?? 'Failed to delete transaction.',
        );
      },
    });
  }

  // Filters

  onSearch(event: Event): void {
    const input = event.target as HTMLInputElement;

    this.searchTerm.set(input.value);
  }

  onTypeChange(event: Event): void {
    const select = event.target as HTMLSelectElement;

    this.selectedType.set(select.value as 'all' | 'income' | 'expense');

    this.pageNumber.set(1);

    this.loadTransactions();
  }

  onCategoryChange(event: Event): void {
    const select = event.target as HTMLSelectElement;
    const value = select.value;
    this.selectedCategory.set(value === 'all' ? 'all' : Number(value));
    this.pageNumber.set(1);
    this.loadTransactions();
  }

  clearFilters(): void {
    this.searchTerm.set('');

    this.selectedType.set('all');

    this.selectedCategory.set('all');

    this.pageNumber.set(1);

    this.loadTransactions();
  }
}
