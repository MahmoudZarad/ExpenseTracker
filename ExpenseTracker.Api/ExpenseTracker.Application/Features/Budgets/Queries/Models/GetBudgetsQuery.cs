using ExpenseTracker.Application.Common.Patterns;
using ExpenseTracker.Application.DTOs.Budget;
using ExpenseTracker.Application.Interfaces.Common;
using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Budgets.Queries.Models
{
    public record GetBudgetsQuery(PaginationParams Params
) : IRequest<Result<PaginatedResult<BudgetDto>>>,
    IHasPagination<PaginationParams>;
}
