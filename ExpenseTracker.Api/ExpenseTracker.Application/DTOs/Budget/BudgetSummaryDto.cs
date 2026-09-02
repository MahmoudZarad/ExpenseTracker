namespace ExpenseTracker.Application.DTOs.Budget
{
    public class BudgetSummaryDto
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string Category { get; set; } = string.Empty;

        public decimal Spent { get; set; }

        public decimal Limit { get; set; }

        public decimal Percentage { get; set; }
    }
}
