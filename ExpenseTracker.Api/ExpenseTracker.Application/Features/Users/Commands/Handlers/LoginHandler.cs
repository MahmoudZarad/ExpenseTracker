using ExpenseTracker.Application.Common.Auth;
using ExpenseTracker.Application.Features.Users.Commands.Models;
using ExpenseTracker.Application.Interfaces.Common.Jwt;
using ExpenseTracker.Application.Interfaces.IUnitOfWork;
using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Users.Commands.Handlers;

public class LoginHandler
    : IRequestHandler<LoginCommand, Result<AuthResponseDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IJwtService _jwt;

    public LoginHandler(
        IUnitOfWork uow,
        IJwtService jwt)
    {
        _uow = uow;
        _jwt = jwt;
    }

    public async Task<Result<AuthResponseDto>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var email = request.Request.Email
            .Trim()
            .ToLowerInvariant();

        var user = await _uow.Users.FindAsync(
            x => x.Email == email,
            cancellationToken);

        if (user is null)
        {
            return Result<AuthResponseDto>
                .Failure("Invalid email or password.");
        }

        var passwordValid =
            BCrypt.Net.BCrypt.Verify(
                request.Request.Password,
                user.PasswordHash);

        if (!passwordValid)
        {
            return Result<AuthResponseDto>
                .Failure("Invalid email or password.");
        }

        var token = _jwt.GenerateToken(user);

        return Result<AuthResponseDto>.Success(
            new AuthResponseDto
            {
                UserId = user.Id,

                Name = user.Name,

                Email = user.Email,

                Token = token
            });
    }
}