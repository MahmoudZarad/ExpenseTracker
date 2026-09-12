using ExpenseTracker.Application.Common.Auth;
using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Users.Commands.Models
{
    public record LoginCommand(
    LoginRequest Request
) : IRequest<Result<AuthResponseDto>>;
}
