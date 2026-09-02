using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Features.Transactions.Queries.Models;
using ExpenseTracker.Application.Interfaces.Common;
using ExpenseTracker.Application.Interfaces.IUnitOfWork;
using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Transactions.Queries.Handlers;

public class GetTransactionByIdQueryHandler
    : IRequestHandler<
        GetTransactionByIdQuery,
        Result<TransactionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetTransactionByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Result<TransactionDto>> Handle(
        GetTransactionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var transaction =
            await _unitOfWork.Transactions.GetByAsync<TransactionDto>(
                x =>
                    x.Id == request.Id &&
                    x.UserId == _currentUserService.UserId,

                _mapper.ConfigurationProvider,
                cancellationToken);

        if (transaction is null)
        {
            return Result<TransactionDto>.Failure(
                "Transaction not found.",
                404);
        }

        return Result<TransactionDto>.Success(
            transaction);
    }
}
