using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Categories.Queries.Models;

public record GetCategoryByIdQuery(
    int Id,
    int UserId
) : IRequest<Result<CategoryDto>>;