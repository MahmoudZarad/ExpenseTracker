using ExpenseTracker.Application.DTOs.Budget;
using ExpenseTracker.Application.Interfaces.IRepositories.IEntitiesRepository;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Enums;
using ExpenseTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Repositeries.EntitiesRepository;

public class BudgetRepository
    : GenericRepository<Budget>,
      IBudgetRepository
{
    public BudgetRepository(ApplicationDbContext context)
        : base(context)
    {
        _context = context;
    }

    private readonly ApplicationDbContext _context;

    public async Task<List<BudgetSummaryDto>> GetSummaryAsync(
    int userId,
    CancellationToken cancellationToken = default)
    {
        return await _context.Budgets
            .AsNoTracking()
            .Where(b => b.UserId == userId)
            .Select(b => new BudgetSummaryDto
            {
                Id = b.Id,

                CategoryId = b.CategoryId,

                Category = b.Category.Name,

                Limit = b.Limit,

                Spent = _context.Transactions
                    .Where(t =>
                        t.UserId == userId &&
                        t.CategoryId == b.CategoryId &&
                        t.Type == TransactionType.Expense)
                    .Sum(t => (decimal?)t.Amount) ?? 0
            })
            .Select(x => new BudgetSummaryDto
            {
                Id = x.Id,

                CategoryId = x.CategoryId,

                Category = x.Category,

                Limit = x.Limit,

                Spent = x.Spent,

                Percentage = x.Limit > 0
                    ? Math.Round(
                        x.Spent / x.Limit * 100,
                        2)
                    : 0
            })
            .ToListAsync(cancellationToken);
    }
}
