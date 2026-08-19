using ExpenseTracker.Application.DTOs.Budget;

namespace ExpenseTracker.Application.DTOs
{
    public class DashboardDto
    {
        public decimal TotalIncome { get; set; }

        public decimal TotalExpense { get; set; }

        public decimal Balance { get; set; }

        public decimal Savings { get; set; }

        public decimal BalanceChangePercentage { get; set; }

        public List<TransactionDto> RecentTransactions { get; set; } = [];

        public List<SpendingSummaryDto> SpendingSummary { get; set; } = [];

        public List<BudgetSummaryDto> BudgetSummary { get; set; } = [];
    }
}
