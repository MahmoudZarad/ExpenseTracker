using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ExpenseTracker.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<DashboardRaw>().HasNoKey();

        // 
        modelBuilder.Entity<DashboardRaw>()
    .Property(x => x.TotalIncome)
    .HasPrecision(18, 2);

        modelBuilder.Entity<DashboardRaw>()
            .Property(x => x.TotalExpense)
            .HasPrecision(18, 2);

        modelBuilder.Entity<DashboardRaw>()
            .Property(x => x.BalanceChangePercentage)
            .HasPrecision(18, 2);
    }

    //public DbSet<User> Users = 
    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Budget> Budgets => Set<Budget>();
    public DbSet<DashboardRaw> DashboardRaw { get; set; }

}
