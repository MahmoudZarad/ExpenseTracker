using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Users.Commands.Models
{
    public record GetCurrentUserQuery
    : IRequest<Result<UserProfileDto>>;
}
