namespace ExpenseTracker.Application.Features.Dashboard
{
    public class BudgetDashboardItem
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public decimal Limit { get; set; }
    }
}
