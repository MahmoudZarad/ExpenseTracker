using ExpenseTracker.Application.Interfaces.IRepositories.IEntitiesRepository;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Data;

namespace ExpenseTracker.Infrastructure.Repositeries.EntitiesRepository;

public class UserRepository
    : GenericRepository<User>,
      IUserRepository
{
    public UserRepository(ApplicationDbContext context)
        : base(context)
    {
    }
}
