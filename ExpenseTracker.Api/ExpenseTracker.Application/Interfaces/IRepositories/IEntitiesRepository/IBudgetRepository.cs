using ExpenseTracker.Application.DTOs.Budget;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Interfaces.IRepositories.IEntitiesRepository;

public interface IBudgetRepository : IGenericRepository<Budget>
{
    Task<List<BudgetSummaryDto>> GetSummaryAsync(
        int userId,
        CancellationToken cancellationToken = default);
}