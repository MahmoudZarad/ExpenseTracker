using ExpenseTracker.Application.Interfaces.IRepositories.IEntitiesRepository;
using ExpenseTracker.Application.Interfaces.IUnitOfWork;
using ExpenseTracker.Application.Interfaces.Repositories.IEntitiesRepository;
using ExpenseTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ExpenseTracker.Infrastructure.Repositeries.UnitOfWork;

public class UnitOfWork(ApplicationDbContext context,
    IUserRepository userRepository,
    ITransactionRepository transactionRepository,
    ICategoryRepository categoryRepository,
    IBudgetRepository budgetRepository) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;
    public IUserRepository Users { get; } = userRepository;
    public ITransactionRepository Transactions { get; } = transactionRepository;
    public ICategoryRepository Categories { get; } = categoryRepository;
    public IBudgetRepository Budgets { get; } = budgetRepository;

    public DatabaseFacade Database => _context.Database;

    public Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);

    public async ValueTask DisposeAsync()
        => await _context.DisposeAsync();

    public void Dispose()
        => _context.Dispose();
}