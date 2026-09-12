using ExpenseTracker.Application.Interfaces.IRepositories.IEntitiesRepository;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Data;

namespace ExpenseTracker.Infrastructure.Repositeries.EntitiesRepository;

public class CategoryRepository
    : GenericRepository<Category>,
      ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context)
        : base(context)
    {
    }
}
