using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Application.Common.Patterns;

public class TransactionQueryParams : PaginationParams
{
    public string? Search { get; set; }

    public TransactionType? Type { get; set; }

    public int? CategoryId { get; set; }
}