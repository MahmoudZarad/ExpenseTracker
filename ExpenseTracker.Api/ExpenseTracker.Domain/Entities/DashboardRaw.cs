namespace ExpenseTracker.Domain.Entities
{
    public class DashboardRaw
    {
        public decimal TotalIncome { get; set; }

        public decimal TotalExpense { get; set; }

        public string RecentTransactions { get; set; } = "[]";
        
        public decimal BalanceChangePercentage { get; set; }

        public string SpendingSummary { get; set; } = "[]";

        public string BudgetSummary { get; set; } = "[]";
    }
}
