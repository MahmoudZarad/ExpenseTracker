using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Budgets.Commands.Models
{
    public record CreateBudgetCommand(
    int CategoryId,
    decimal Limit
) : IRequest<Result<int>>;
}
