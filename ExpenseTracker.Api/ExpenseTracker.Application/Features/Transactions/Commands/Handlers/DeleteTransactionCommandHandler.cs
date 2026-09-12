using ExpenseTracker.Application.Features.Transactions.Commands.Models;
using ExpenseTracker.Application.Interfaces.Common;
using ExpenseTracker.Application.Interfaces.IUnitOfWork;
using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Transactions.Commands.Handlers;

public class DeleteTransactionCommandHandler
    : IRequestHandler<
        DeleteTransactionCommand,
        Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public DeleteTransactionCommandHandler(
        IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<bool>> Handle(
        DeleteTransactionCommand request,
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

        _unitOfWork.Transactions.Remove(
            transaction);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result<bool>.Success(true);
    }
}
