using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Categories.Commands.Models
{
    public record DeleteCategoryCommand(
    int Id) : IRequest<Result<bool>>;
}
