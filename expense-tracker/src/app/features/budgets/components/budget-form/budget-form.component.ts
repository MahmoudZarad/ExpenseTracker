import {
  Component,
  EventEmitter,
  inject,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';

import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { Budget } from '../../../../core/models/budget.model';
import {
  Category,
  TransactionType,
} from '../../../../core/models/category.model';
import { CategoriesService } from '../../../categories/services/categories.service';
import { CurrencyService } from '../../../../core/services/CurrencyService.service';

@Component({
  selector: 'app-budget-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './budget-form.component.html',
  styleUrl: './budget-form.component.css',
})
export class BudgetFormComponent implements OnInit, OnChanges {
  @Input()
  budget: Budget | null = null;

  @Output()
  budgetSaved = new EventEmitter<Budget>();

  protected readonly currencyService = inject(CurrencyService);
  private readonly categoriesService = inject(CategoriesService);

  categories: Category[] = [];

  readonly form = this.fb.nonNullable.group({
    categoryId: [null as number | null, Validators.required],
    limit: [0, [Validators.required, Validators.min(1)]],
  });

  constructor(private readonly fb: FormBuilder) {}

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {
    this.categoriesService.getAll().subscribe({
      next: (response) => {
        this.categories = response.items.filter(
          (category) => category.type === TransactionType.Expense,
        );

        if (this.budget) {
          this.form.patchValue({
            categoryId: this.budget.categoryId,
            limit: this.budget.limit,
          });
          return;
        }

        this.setDefaultCategory();
      },
    });
  }

  private setDefaultCategory(): void {
    if (this.budget) {
      return;
    }

    if (!this.categories.length) {
      this.form.get('categoryId')?.setValue(null);
      return;
    }

    const selectedCategoryId = this.form.get('categoryId')?.value;
    const exists = this.categories.some(
      (category) => category.id === Number(selectedCategoryId),
    );

    if (!exists) {
      this.form.get('categoryId')?.setValue(this.categories[0].id);
    }
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const categoryId = Number(this.form.value.categoryId);
    const budget: Budget = {
      id: this.budget?.id ?? 0,
      categoryId,
      categoryName:
        this.categories.find((category) => category.id === categoryId)?.name ??
        '',
      limit: Number(this.form.value.limit),
    };

    this.budgetSaved.emit(budget);

    if (!this.budget) {
      this.form.reset({
        categoryId: this.categories[0]?.id ?? null,
        limit: 0,
      });
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['budget'] && this.budget) {
      this.form.patchValue({
        categoryId: this.budget.categoryId,
        limit: this.budget.limit,
      });
      return;
    }

    if (changes['budget'] && !this.budget) {
      this.setDefaultCategory();
    }
  }
}
