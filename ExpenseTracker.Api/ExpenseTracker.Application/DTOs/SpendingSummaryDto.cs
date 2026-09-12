namespace ExpenseTracker.Application.DTOs
{
    public class SpendingSummaryDto
    {
        public string Label { get; set; } = string.Empty;

        public decimal Amount { get; set; }
    }
}
