using ExpenseTracker.Application.Common.Auth;
using ExpenseTracker.Application.Common.Email;
using ExpenseTracker.Application.Features.Users.Commands.Models;
using ExpenseTracker.Application.Interfaces.Common.Jwt;
using ExpenseTracker.Application.Interfaces.ExternalServices;
using ExpenseTracker.Application.Interfaces.IUnitOfWork;
using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Enums;
using MediatR;

namespace ExpenseTracker.Application.Features.Auth.Commands.Handlers;

public class RegisterHandler
    : IRequestHandler<RegisterCommand, Result<AuthResponseDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IEmailService _email;
    private readonly IJwtService _jwt;

    public RegisterHandler(
        IUnitOfWork uow,
        IEmailService email,
        IJwtService jwt)
    {
        _uow = uow;
        _email = email;
        _jwt = jwt;
    }

    public async Task<Result<AuthResponseDto>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var fullName = request.Request.FullName.Trim();

        var email = request.Request.Email
            .Trim()
            .ToLowerInvariant();

        if (request.Request.Password != request.Request.ConfirmPassword)
            return Result<AuthResponseDto>.Failure("Passwords do not match.");

        var emailExists = await _uow.Users.AnyAsync(
            x => x.Email == email, cancellationToken);

        if (emailExists)
        {
            return Result<AuthResponseDto>
                .Failure("Email is already registered.");
        }

        await using var transaction =
            await _uow.Database.BeginTransactionAsync(
                cancellationToken);

        try
        {
            var user = new User
            {
                Name = fullName,

                Email = email,

                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Request.Password)
            };

            await _uow.Users.AddAsync(
                user, cancellationToken);

            await _uow.SaveChangesAsync(cancellationToken);

            await CreateDefaultCategoriesAsync(user.Id, cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            var token = _jwt.GenerateToken(user);

            // Email Notification (Best effort, do not fail registration if email fails)

            try
            {
                await _email.SendEmailAsync(new EmailDto
                {
                    To = user.Email,

                    Subject =
                            "Welcome to ExpenseTracker",

                    Body = $"""
                            <h2>Welcome {user.Name}! 👋</h2>

                            <p>
                                Your ExpenseTracker account
                                has been created successfully.
                            </p>

                            <p>
                                You can now start tracking
                                your income, expenses and budgets.
                            </p>
                            """
                });
            }
            catch
            {
                // Account creation already succeeded.
                // Email failure should not affect registration.
            }

            return Result<AuthResponseDto>.Success(
                new AuthResponseDto
                {
                    UserId = user.Id,

                    Name = user.Name,

                    Email = user.Email,

                    Token = token
                });
        }
        catch
        {
            await transaction.RollbackAsync(
                cancellationToken);

            return Result<AuthResponseDto>
                .Failure("Registration failed.");
        }
    }

    private async Task CreateDefaultCategoriesAsync(
        int userId,
        CancellationToken cancellationToken)
    {
        var categories = new List<Category>
        {
            // Expenses

            new()
            {
                Name = "Food",
                Type = TransactionType.Expense,
                UserId = userId
            },

            new()
            {
                Name = "Transportation",
                Type = TransactionType.Expense,
                UserId = userId
            },

            new()
            {
                Name = "Shopping",
                Type = TransactionType.Expense,
                UserId = userId
            },

            new()
            {
                Name = "Bills",
                Type = TransactionType.Expense,
                UserId = userId
            },

            new()
            {
                Name = "Entertainment",
                Type = TransactionType.Expense,
                UserId = userId
            },

            new()
            {
                Name = "Health",
                Type = TransactionType.Expense,
                UserId = userId
            },

            new()
            {
                Name = "Education",
                Type = TransactionType.Expense,
                UserId = userId
            },

            new()
            {
                Name = "Other",
                Type = TransactionType.Expense,
                UserId = userId
            },

            // Income

            new()
            {
                Name = "Salary",
                Type = TransactionType.Income,
                UserId = userId
            },

            new()
            {
                Name = "Freelance",
                Type = TransactionType.Income,
                UserId = userId
            },

            new()
            {
                Name = "Investment",
                Type = TransactionType.Income,
                UserId = userId
            },

            new()
            {
                Name = "Other Income",
                Type = TransactionType.Income,
                UserId = userId
            }
        };

        await _uow.Categories.AddRangeAsync(
            categories,
            cancellationToken);

        await _uow.SaveChangesAsync(
            cancellationToken);
    }
}