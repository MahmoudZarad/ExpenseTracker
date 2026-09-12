using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Features.Users.Commands.Models;
using ExpenseTracker.Application.Interfaces.Common;
using ExpenseTracker.Application.Interfaces.IUnitOfWork;
using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Users.Commands.Handlers;

public class GetCurrentUserQueryHandler
    : IRequestHandler<
        GetCurrentUserQuery,
        Result<UserProfileDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetCurrentUserQueryHandler(
        IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<Result<UserProfileDto>> Handle(
        GetCurrentUserQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _uow.Users.FindAsync(
            x => x.Id == _currentUser.UserId,
            cancellationToken);

        if (user is null)
            return Result<UserProfileDto>.Failure("User not found.");

        return Result<UserProfileDto>.Success(
            new UserProfileDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Currency = user.Currency,
                Language = user.Language
            });
    }
}