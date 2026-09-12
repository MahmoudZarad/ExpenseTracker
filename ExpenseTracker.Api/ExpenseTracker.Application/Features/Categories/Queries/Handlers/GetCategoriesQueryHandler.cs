using AutoMapper;
using ExpenseTracker.Application.Common.Patterns;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Features.Categories.Queries.Models;
using ExpenseTracker.Application.Interfaces.Common;
using ExpenseTracker.Application.Interfaces.IUnitOfWork;
using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Entities;
using MediatR;

namespace ExpenseTracker.Application.Features.Categories.Queries.Handlers;

public class GetCategoriesQueryHandler
    : IRequestHandler<
        GetCategoriesQuery,
        Result<PaginatedResult<CategoryDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetCategoriesQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Result<PaginatedResult<CategoryDto>>> Handle(
        GetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        var options = new QueryOptions<Category>
        {
            Filter = x => x.UserId == userId,

            OrderBy = x => x.Name,

            Descending = false,

            Skip = (request.Params.PageNumber - 1)
                   * request.Params.PageSize,

            Take = request.Params.PageSize,

            Tracking = false
        };

        var items =
            await _unitOfWork.Categories
                .GetAllAsync<CategoryDto>(
                    options,
                    _mapper.ConfigurationProvider,
                    cancellationToken);

        var totalCount =
            await _unitOfWork.Categories.CountAsync(
                x => x.UserId == userId,
                cancellationToken);

        var result = new PaginatedResult<CategoryDto>
        {
            Items = items,
            PageNumber = request.Params.PageNumber,
            PageSize = request.Params.PageSize,
            TotalCount = totalCount
        };

        return Result<PaginatedResult<CategoryDto>>
            .Success(result);
    }
}