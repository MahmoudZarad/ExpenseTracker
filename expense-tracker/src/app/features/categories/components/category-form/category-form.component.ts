import {
  Component,
  EventEmitter,
  Input,
  Output,
  SimpleChanges,
} from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import {
  Category,
  TransactionType,
} from '../../../../core/models/category.model';

@Component({
  selector: 'app-category-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './category-form.component.html',
  styleUrl: './category-form.component.css',
})
export class CategoryFormComponent {
  @Input() category: Category | null = null;

  @Output() categorySaved = new EventEmitter<Category>();
  @Output() cancelled = new EventEmitter<void>();

  readonly TransactionType = TransactionType;

  readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.minLength(2)]],
    type: [TransactionType.Expense, Validators.required],
  });

  constructor(private readonly fb: FormBuilder) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['category'] && this.category) {
      this.form.patchValue({
        name: this.category.name,
        type: Number(this.category.type),
      });
    }
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const category: Category = {
      id: this.category ? this.category.id : Date.now(),
      name: this.form.getRawValue().name.trim(),
      type: Number(this.form.getRawValue().type) as TransactionType,
    };

    this.categorySaved.emit(category);
  }

  cancel(): void {
    this.cancelled.emit();
  }
}
