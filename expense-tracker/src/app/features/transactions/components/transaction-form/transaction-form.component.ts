import { ToastMessageService } from './../../../../core/services/toast-message.service';
import {
  Component,
  EventEmitter,
  inject,
  Input,
  OnChanges,
  OnInit,
  Output,
  signal,
  SimpleChanges,
} from '@angular/core';

import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { Transaction } from '../../../../core/models/transaction.module';
import { CategoriesService } from '../../../categories/services/categories.service';
import {
  Category,
  TransactionType,
} from '../../../../core/models/category.model';
import { CurrencyService } from '../../../../core/services/CurrencyService.service';

@Component({
  selector: 'app-transaction-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './transaction-form.component.html',
  styleUrls: ['./transaction-form.component.css'],
})
export class TransactionFormComponent implements OnInit, OnChanges {
  @Output() transactionCreated = new EventEmitter<Transaction>();
  @Input() transaction: Transaction | null = null;

  private readonly categoriesService = inject(CategoriesService);
  protected readonly currencyService = inject(CurrencyService);
  private readonly toastMessageService = inject(ToastMessageService);

  readonly TransactionType = TransactionType;

  categories = signal<Category[]>([]);

  readonly form = this.fb.nonNullable.group({
    title: [
      '',
      [Validators.required, Validators.minLength(2), Validators.maxLength(50)],
    ],
    description: ['', Validators.maxLength(150)],
    amount: [0, [Validators.required, Validators.min(1)]],
    type: [TransactionType.Expense, Validators.required],
    categoryId: [null as number | null, Validators.required],
    date: [this.getToday(), Validators.required],
  });

  constructor(private readonly fb: FormBuilder) {}

  ngOnInit(): void {
    this.loadCategories();
    this.form.get('type')?.valueChanges.subscribe((value) => {
      const normalizedType = Number(value ?? TransactionType.Expense);
      if (Number(this.form.get('type')?.value) !== normalizedType) {
        this.form.patchValue({ type: normalizedType }, { emitEvent: false });
      }
      this.setDefaultCategory();
    });
  }

  get categoriesByType(): Category[] {
    const type = Number(
      this.form.get('type')?.value ?? TransactionType.Expense,
    );
    return this.categories().filter((category) => category.type === type);
  }

  loadCategories(): void {
    this.categoriesService.getAll().subscribe({
      next: (response) => {
        this.categories.set(response.items);
        this.setDefaultCategory();
      },
      error: () => {
        this.toastMessageService.showMessage('Failed to load categories.');
      },
    });
  }

  private setDefaultCategory(): void {
    const currentCategoryId = this.form.get('categoryId')?.value;
    const exists = this.categoriesByType.some(
      (category) => category.id === Number(currentCategoryId),
    );

    if (exists) {
      return;
    }

    const firstCategory = this.categoriesByType[0];
    this.form.get('categoryId')?.setValue(firstCategory?.id ?? null);
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const payload = this.form.getRawValue();
    const transaction: Transaction = {
      id: this.transaction?.id ?? 0,
      title: payload.title,
      description: payload.description ?? null,
      amount: Number(payload.amount),
      type: Number(payload.type) as TransactionType,
      categoryId: Number(payload.categoryId),
      date: payload.date,
      categoryName: null,
    };

    this.transactionCreated.emit(transaction);

    if (!this.transaction) {
      this.resetForm();
    }
  }

  private resetForm(): void {
    this.form.reset({
      title: '',
      description: '',
      amount: 0,
      type: TransactionType.Expense,
      categoryId:
        this.categories().find(
          (category) => category.type === TransactionType.Expense,
        )?.id ?? null,
      date: this.getToday(),
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['transaction'] && this.transaction) {
      this.form.patchValue({
        title: this.transaction.title,
        description: this.transaction.description ?? '',
        amount: this.transaction.amount,
        type: Number(this.transaction.type),
        categoryId: this.transaction.categoryId,
        date: this.normalizeDateForInput(this.transaction.date),
      });
    }
  }

  private normalizeDateForInput(value: string | null | undefined): string {
    if (!value) {
      return this.getToday();
    }

    const rawDate = value.split('T')[0];
    if (/^\d{4}-\d{2}-\d{2}$/.test(rawDate)) {
      return rawDate;
    }

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) {
      return this.getToday();
    }

    return new Date(date.getTime() - date.getTimezoneOffset() * 60000)
      .toISOString()
      .split('T')[0];
  }

  private getToday(): string {
    return new Date().toISOString().split('T')[0];
  }
}
