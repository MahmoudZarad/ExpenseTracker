namespace ExpenseTracker.Domain.Entities;

public class Budget
{
    public int Id { get; set; }

    public decimal Limit { get; set; }

    public int CategoryId { get; set; }

    public int UserId { get; set; }

    public Category Category { get; set; } = null!;

    public User User { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
