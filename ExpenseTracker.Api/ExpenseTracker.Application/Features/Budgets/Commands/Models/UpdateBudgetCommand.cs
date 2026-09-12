using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Budgets.Commands.Models
{
    public record UpdateBudgetCommand(
    int Id,
    int CategoryId,
    decimal Limit
) : IRequest<Result<bool>>;
}
