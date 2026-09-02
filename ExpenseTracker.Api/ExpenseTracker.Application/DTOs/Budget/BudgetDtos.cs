namespace ExpenseTracker.Application.DTOs.Budget;

public class BudgetDto
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public decimal Limit { get; set; }

    public decimal Spent { get; set; }

    public decimal Percentage { get; set; }
}
