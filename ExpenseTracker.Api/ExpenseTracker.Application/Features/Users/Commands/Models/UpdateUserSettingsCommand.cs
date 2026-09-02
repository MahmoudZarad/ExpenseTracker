using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Users.Commands.Models;

public record UpdateUserSettingsCommand(
    UpdateUserSettingsRequest Request)
    : IRequest<Result<UserProfileDto>>;