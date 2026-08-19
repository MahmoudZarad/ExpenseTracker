using ExpenseTracker.Application.DTOs.Budget;
using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Budgets.Queries.Models
{
    public record GetBudgetByIdQuery(
    int Id) : IRequest<Result<BudgetDto>>;
}
