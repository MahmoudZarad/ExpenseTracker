using ExpenseTracker.Application.Interfaces.Repositories.IEntitiesRepository;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Data;

namespace ExpenseTracker.Infrastructure.Repositeries.EntitiesRepository;

public class TransactionRepository
    : GenericRepository<Transaction>,
      ITransactionRepository
{
    public TransactionRepository(ApplicationDbContext context)
        : base(context)
    {
    }
}