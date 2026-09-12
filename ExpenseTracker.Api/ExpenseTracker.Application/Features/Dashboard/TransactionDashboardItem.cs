using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Application.Features.Dashboard
{
    public class TransactionDashboardItem
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public TransactionType Type { get; set; }

        public DateTime Date { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;
    }
}
