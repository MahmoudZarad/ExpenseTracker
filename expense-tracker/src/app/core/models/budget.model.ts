export interface Budget {
  id: number;
  categoryId: number;
  categoryName: string | null;
  limit: number;
  spent?: number;
  percentage?: number;
}

export interface CreateBudget {
  categoryId: number;
  limit: number;
}

export interface UpdateBudget extends CreateBudget {
  id: number;
}

export interface BudgetSummary {
  id: number;
  categoryId: number;
  category: string | null;
  spent: number;
  limit: number;
  percentage: number;
}
