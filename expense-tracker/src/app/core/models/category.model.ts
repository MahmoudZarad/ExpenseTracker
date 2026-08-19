export enum TransactionType {
  Income = 1,
  Expense = 2,
}

export interface Category {
  id: number;
  name: string;
  type: TransactionType;
}

export interface CreateCategory {
  name: string;
  type: TransactionType;
}

export interface UpdateCategory {
  id: number;
  name: string;
  type: TransactionType;
}
