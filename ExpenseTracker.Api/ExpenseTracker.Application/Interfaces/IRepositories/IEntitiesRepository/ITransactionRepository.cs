using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Interfaces.Repositories.IEntitiesRepository;

public interface ITransactionRepository
    : IGenericRepository<Transaction>
{
    // Transaction-specific methods
}