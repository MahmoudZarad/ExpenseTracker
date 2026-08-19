using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Features.Users.Commands.Models;
using ExpenseTracker.Application.Interfaces.Common;
using ExpenseTracker.Application.Interfaces.IUnitOfWork;
using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Users.Commands.Handlers;

public class UpdateUserSettingsCommandHandler
    : IRequestHandler<
        UpdateUserSettingsCommand,
        Result<UserProfileDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public UpdateUserSettingsCommandHandler(
        IUnitOfWork uow,
        ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<Result<UserProfileDto>> Handle(
        UpdateUserSettingsCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _uow.Users.FindAsync(
            x => x.Id == _currentUser.UserId,
            cancellationToken);

        if (user is null)
            return Result<UserProfileDto>.Failure("User not found.");

        var name = request.Request.Name.Trim();

        var email = request.Request.Email
            .Trim().ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(name))
            return Result<UserProfileDto>.Failure("Name is required.");

        if (string.IsNullOrWhiteSpace(email))
        {
            return Result<UserProfileDto>
                .Failure("Email is required.");
        }

        var emailExists = await _uow.Users.AnyAsync(
            x =>
                x.Id != user.Id &&
                x.Email == email,
            cancellationToken);

        if (emailExists)
        {
            return Result<UserProfileDto>
                .Failure("Email is already in use.");
        }

        user.Name = name;
        user.Email = email;
        user.Currency = request.Request.Currency;
        user.Language = request.Request.Language;

        await _uow.SaveChangesAsync(
            cancellationToken);

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