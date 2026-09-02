using ExpenseTracker.Application.DTOs.Budget;
using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Budgets.Queries.Models
{
    public record GetBudgetSummaryQuery() : IRequest<Result<List<BudgetSummaryDto>>>;
}
