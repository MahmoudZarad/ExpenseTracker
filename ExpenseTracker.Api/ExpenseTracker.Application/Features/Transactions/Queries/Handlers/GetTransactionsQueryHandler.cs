using AutoMapper;
using ExpenseTracker.Application.Common.Patterns;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Features.Transactions.Queries.Models;
using ExpenseTracker.Application.Interfaces.Common;
using ExpenseTracker.Application.Interfaces.IUnitOfWork;
using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Entities;
using MediatR;

namespace ExpenseTracker.Application.Features.Transactions.Queries.Handlers;

public class GetTransactionsQueryHandler
    : IRequestHandler<
        GetTransactionsQuery,
        Result<PaginatedResult<TransactionDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;


    public GetTransactionsQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Result<PaginatedResult<TransactionDto>>> Handle(
    GetTransactionsQuery request,
    CancellationToken cancellationToken)
    {
        var options = new QueryOptions<Transaction>
        {
            Filter = x =>
                x.UserId == _currentUserService.UserId &&

                (
                    string.IsNullOrEmpty(request.Params.Search) ||
                    x.Title.Contains(request.Params.Search) ||
                    x.Description.Contains(request.Params.Search) ||
                    x.Category.Name.Contains(request.Params.Search)
                ) &&

                (
                    request.Params.Type == null ||
                    x.Type == request.Params.Type
                ) &&

                (
                    request.Params.CategoryId == null ||
                    x.CategoryId == request.Params.CategoryId
                ),

            OrderBy = x => x.Date,

            Descending = true,

            Skip =
                (request.Params.PageNumber - 1)
                * request.Params.PageSize,

            Take =
                request.Params.PageSize,

            Tracking = false
        };

        var items =
            await _unitOfWork.Transactions
                .GetAllAsync<TransactionDto>(
                    options,
                    _mapper.ConfigurationProvider,
                    cancellationToken);

        var totalCount =
            await _unitOfWork.Transactions.CountAsync(
                options.Filter,
                cancellationToken);

        var result = new PaginatedResult<TransactionDto>
        {
            Items = items,
            PageNumber = request.Params.PageNumber,
            PageSize = request.Params.PageSize,
            TotalCount = totalCount
        };

        return Result<PaginatedResult<TransactionDto>>
            .Success(result);
    }
}