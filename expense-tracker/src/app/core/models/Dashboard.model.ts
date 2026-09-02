import { BudgetSummary } from './budget.model';
import { SpendingSummary } from './spending-summary.model';
import { Transaction } from './transaction.module';

export interface Dashboard {
  totalIncome: number;
  totalExpense: number;
  balance: number;
  savings: number;
  balanceChangePercentage: number;
  recentTransactions: Transaction[];
  spendingSummary: SpendingSummary[];
  budgetSummary: BudgetSummary[];
}
