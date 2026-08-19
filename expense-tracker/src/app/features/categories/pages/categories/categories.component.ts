import { Component, inject, signal } from '@angular/core';

import { Category } from '../../../../core/models/category.model';
import { CategoryFormComponent } from '../../components/category-form/category-form.component';
import { CategoriesService } from '../../services/categories.service';
import { ToastMessageService } from '../../../../core/services/toast-message.service';

@Component({
  selector: 'app-categories',
  standalone: true,
  imports: [CategoryFormComponent],
  templateUrl: './categories.component.html',
  styleUrl: './categories.component.css',
})
export class CategoriesComponent {
  private readonly categoriesService = inject(CategoriesService);

  private readonly toastMessage = inject(ToastMessageService);

  categories = signal<Category[]>([]);

  isLoading = signal(false);

  isFormOpen = signal(false);

  selectedCategory = signal<Category | null>(null);

  ngOnInit(): void {
    this.loadCategories();
  }

  // =========================
  // GET
  // =========================

  loadCategories(): void {
    this.isLoading.set(true);

    this.categoriesService.getAll().subscribe({
      next: (response) => {
        console.log('CATEGORIES RESPONSE:', response);

        this.categories.set(response.items);

        this.isLoading.set(false);
      },

      error: (error) => {
        this.isLoading.set(false);

        console.error('Failed to load categories:', error);

        this.toastMessage.showMessage(
          error?.error?.message ?? 'Failed to load categories.',
        );
      },
    });
  }

  // =========================
  // Form
  // =========================

  openForm(): void {
    this.selectedCategory.set(null);

    this.isFormOpen.set(true);
  }

  closeForm(): void {
    this.isFormOpen.set(false);

    this.selectedCategory.set(null);
  }

  editCategory(category: Category): void {
    this.selectedCategory.set(category);

    this.isFormOpen.set(true);
  }

  // =========================
  // CREATE / UPDATE
  // =========================

  saveCategory(category: Category): void {
    const selected = this.selectedCategory();

    if (selected) {
      this.categoriesService
        .update({
          id: category.id,
          name: category.name,
          type: category.type,
        })
        .subscribe({
          next: () => {
            this.toastMessage.showMessage('Category updated successfully.');
            this.closeForm();
            this.loadCategories();
          },
          error: (error) => {
            this.toastMessage.showMessage(
              error?.error?.message ?? 'Failed to update category.',
            );
          },
        });

      return;
    }

    this.categoriesService
      .create({
        name: category.name,
        type: category.type,
      })
      .subscribe({
        next: () => {
          this.toastMessage.showMessage('Category created successfully.');
          this.closeForm();
          this.loadCategories();
        },
        error: (error) => {
          this.toastMessage.showMessage(
            error?.error?.message ?? 'Failed to create category.',
          );
        },
      });
  }

  // =========================
  // DELETE
  // =========================

  deleteCategory(id: number): void {
    const category = this.categories().find((x) => x.id === id);

    if (!category) {
      return;
    }

    const confirmed = confirm(
      `Are you sure you want to delete "${category.name}"?`,
    );

    if (!confirmed) {
      return;
    }

    this.categoriesService.delete(id).subscribe({
      next: () => {
        this.toastMessage.showMessage('Category deleted successfully.');

        this.loadCategories();
      },

      error: (error) => {
        this.toastMessage.showMessage(
          error?.error?.message ?? 'Cannot delete this category.',
        );
      },
    });
  }
}
