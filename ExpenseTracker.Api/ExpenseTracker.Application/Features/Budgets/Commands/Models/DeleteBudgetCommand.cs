using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Budgets.Commands.Models
{
    public record DeleteBudgetCommand(
    int Id
) : IRequest<Result<bool>>;
}
