using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Domain.Entities;

public class User
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Currency { get; set; } = "EGP";

    public string Language { get; set; } = "English";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Transaction> Transactions { get; set; }
        = [];

    public ICollection<Category> Categories { get; set; }
        = [];

    public ICollection<Budget> Budgets { get; set; }
        = [];
}
