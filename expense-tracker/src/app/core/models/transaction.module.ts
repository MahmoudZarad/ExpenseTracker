export { TransactionType } from './category.model';

import { TransactionType } from './category.model';

export interface Transaction {
  id: number;
  title: string;
  description: string | null;
  amount: number;
  type: TransactionType;
  date: string;
  categoryId: number;
  categoryName: string | null;
}

export interface CreateTransaction {
  title: string;
  description: string | null;
  amount: number;
  type: TransactionType;
  date: string;
  categoryId: number;
}

export interface UpdateTransaction extends CreateTransaction {
  id: number;
}
