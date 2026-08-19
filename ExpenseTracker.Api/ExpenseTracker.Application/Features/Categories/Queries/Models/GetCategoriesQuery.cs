using ExpenseTracker.Application.Common.Patterns;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Interfaces.Common;
using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Categories.Queries.Models;

public record GetCategoriesQuery(
    PaginationParams Params
) : IRequest<Result<PaginatedResult<CategoryDto>>>,
    IHasPagination<PaginationParams>;