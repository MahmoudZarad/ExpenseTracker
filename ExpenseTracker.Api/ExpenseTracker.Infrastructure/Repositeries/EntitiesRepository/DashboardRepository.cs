using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.DTOs.Budget;
using ExpenseTracker.Application.Interfaces.IRepositories.IEntitiesRepository;
using ExpenseTracker.Domain.Enums;
using ExpenseTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Repositeries.EntitiesRepository;

public class DashboardRepository(ApplicationDbContext context)
    : IDashboardRepository
{
    public async Task<DashboardDto> GetDashboardAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        // =========================================================
        // 1. TOTAL INCOME
        // =========================================================

        var totalIncome = await context.Transactions
            .Where(x =>
                x.UserId == userId &&
                x.Type == TransactionType.Income)
            .SumAsync(
                x => (decimal?)x.Amount,
                cancellationToken) ?? 0;


        // =========================================================
        // 2. TOTAL EXPENSE
        // =========================================================

        var totalExpense = await context.Transactions
            .Where(x =>
                x.UserId == userId &&
                x.Type == TransactionType.Expense)
            .SumAsync(
                x => (decimal?)x.Amount,
                cancellationToken) ?? 0;


        // =========================================================
        // 3. BALANCE
        // =========================================================

        var balance = totalIncome - totalExpense;


        // =========================================================
        // 4. CURRENT / PREVIOUS MONTH
        // =========================================================

        var currentMonth = new DateTime(
            DateTime.UtcNow.Year,
            DateTime.UtcNow.Month,
            1);

        var nextMonth = currentMonth.AddMonths(1);

        var previousMonth = currentMonth.AddMonths(-1);


        // =========================================================
        // CURRENT MONTH BALANCE
        // =========================================================

        var currentMonthIncome = await context.Transactions
            .Where(x =>
                x.UserId == userId &&
                x.Type == TransactionType.Income &&
                x.Date >= currentMonth &&
                x.Date < nextMonth)
            .SumAsync(
                x => (decimal?)x.Amount,
                cancellationToken) ?? 0;


        var currentMonthExpense = await context.Transactions
            .Where(x =>
                x.UserId == userId &&
                x.Type == TransactionType.Expense &&
                x.Date >= currentMonth &&
                x.Date < nextMonth)
            .SumAsync(
                x => (decimal?)x.Amount,
                cancellationToken) ?? 0;


        var currentMonthBalance =
            currentMonthIncome - currentMonthExpense;


        // =========================================================
        // PREVIOUS MONTH BALANCE
        // =========================================================

        var previousMonthIncome = await context.Transactions
            .Where(x =>
                x.UserId == userId &&
                x.Type == TransactionType.Income &&
                x.Date >= previousMonth &&
                x.Date < currentMonth)
            .SumAsync(
                x => (decimal?)x.Amount,
                cancellationToken) ?? 0;


        var previousMonthExpense = await context.Transactions
            .Where(x =>
                x.UserId == userId &&
                x.Type == TransactionType.Expense &&
                x.Date >= previousMonth &&
                x.Date < currentMonth)
            .SumAsync(
                x => (decimal?)x.Amount,
                cancellationToken) ?? 0;


        var previousMonthBalance =
            previousMonthIncome - previousMonthExpense;


        // =========================================================
        // 5. BALANCE CHANGE %
        // =========================================================

        decimal balanceChangePercentage = 0;

        if (previousMonthBalance != 0)
        {
            balanceChangePercentage =
                (currentMonthBalance - previousMonthBalance)
                / Math.Abs(previousMonthBalance)
                * 100;
        }


        // =========================================================
        // 6. RECENT TRANSACTIONS
        // =========================================================

        var recentTransactions = await context.Transactions
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.Date)
            .ThenByDescending(x => x.Id)
            .Take(5)
            .Select(x => new TransactionDto
            {
                Id = x.Id,

                Title = x.Title,

                Description = x.Description,

                Amount = x.Amount,

                Type = x.Type,

                Date = x.Date,

                CategoryId = x.CategoryId,

                CategoryName = x.Category.Name
            })
            .ToListAsync(cancellationToken);


        // =========================================================
        // 7. SPENDING SUMMARY - LAST 6 MONTHS
        // =========================================================

        var sixMonthsAgo = currentMonth.AddMonths(-5);

        var spendingData = await context.Transactions
            .AsNoTracking()
            .Where(x =>
                x.UserId == userId &&
                x.Type == TransactionType.Expense &&
                x.Date >= sixMonthsAgo &&
                x.Date < nextMonth)
            .GroupBy(x => new
            {
                x.Date.Year,
                x.Date.Month
            })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                Amount = g.Sum(x => x.Amount)
            })
            .ToListAsync(cancellationToken);


        var spendingSummary = new List<SpendingSummaryDto>();

        for (var i = 0; i < 6; i++)
        {
            var month = currentMonth.AddMonths(i - 5);

            var item = spendingData.FirstOrDefault(x =>
                x.Year == month.Year &&
                x.Month == month.Month);

            spendingSummary.Add(new SpendingSummaryDto
            {
                Label = month.ToString("MMM yyyy"),

                Amount = item?.Amount ?? 0
            });
        }


        // =========================================================
        // 8. BUDGET SUMMARY
        // =========================================================

        var budgetSummary = await context.Budgets
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Take(4)
            .Select(x => new BudgetSummaryDto
            {
                Id = x.Id,

                CategoryId = x.CategoryId,

                Category = x.Category.Name,

                Limit = x.Limit,

                Spent = context.Transactions
                    .Where(t =>
                        t.UserId == userId &&
                        t.CategoryId == x.CategoryId &&
                        t.Type == TransactionType.Expense &&
                        t.Date >= currentMonth &&
                        t.Date < nextMonth)
                    .Sum(t => (decimal?)t.Amount) ?? 0
            })
            .ToListAsync(cancellationToken);


        // =========================================================
        // 9. BUDGET PERCENTAGE
        // =========================================================

        foreach (var budget in budgetSummary)
        {
            budget.Percentage =
                budget.Limit == 0
                    ? 0
                    : budget.Spent / budget.Limit * 100;
        }


        // =========================================================
        // 10. RETURN DASHBOARD
        // =========================================================

        return new DashboardDto
        {
            TotalIncome = totalIncome,

            TotalExpense = totalExpense,

            Balance = balance,

            Savings = balance,

            BalanceChangePercentage =
                balanceChangePercentage,

            RecentTransactions =
                recentTransactions,

            SpendingSummary =
                spendingSummary,

            BudgetSummary =
                budgetSummary
        };
    }
}