using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Interfaces.IRepositories.IEntitiesRepository;

public interface ICategoryRepository
    : IGenericRepository<Category>
{
    // Category-specific methods
}
