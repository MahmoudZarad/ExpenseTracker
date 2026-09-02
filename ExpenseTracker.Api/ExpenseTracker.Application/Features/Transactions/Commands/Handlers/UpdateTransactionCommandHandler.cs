using AutoMapper;
using ExpenseTracker.Application.Features.Transactions.Commands.Models;
using ExpenseTracker.Application.Interfaces.Common;
using ExpenseTracker.Application.Interfaces.IUnitOfWork;
using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Transactions.Commands.Handlers;

public class UpdateTransactionCommandHandler
    : IRequestHandler<
        UpdateTransactionCommand,
        Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public UpdateTransactionCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Result<bool>> Handle(
        UpdateTransactionCommand request,
        CancellationToken cancellationToken)
    {
        var transaction =
            await _unitOfWork.Transactions.FindAsync(
                x =>
                    x.Id == request.Id &&
                    x.UserId == _currentUserService.UserId,
                cancellationToken);

        if (transaction is null)
        {
            return Result<bool>.Failure(
                "Transaction not found.",
                404);
        }

        var category =
            await _unitOfWork.Categories.GetByAsync(
                x =>
                    x.Id == request.CategoryId &&
                    x.UserId == _currentUserService.UserId,

                x => new
                {
                    x.Id,
                    x.Type
                },
                cancellationToken);

        if (category is null)
        {
            return Result<bool>.Failure(
                "Category not found.",
                404);
        }

        if (category.Type != request.Type)
        {
            return Result<bool>.Failure(
                "Transaction type must match category type.",
                400);
        }

        _mapper.Map(request, transaction);

        await _unitOfWork.Transactions.UpdateAsync(transaction, cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result<bool>.Success(true);
    }
}
