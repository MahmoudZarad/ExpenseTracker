using ExpenseTracker.Application.Features.Budgets.Commands.Models;
using ExpenseTracker.Application.Interfaces.Common;
using ExpenseTracker.Application.Interfaces.IUnitOfWork;
using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Budgets.Commands.Handlers
{
    public class DeleteBudgetCommandHandler
    : IRequestHandler<
        DeleteBudgetCommand,
        Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeleteBudgetCommandHandler(
            IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(
            DeleteBudgetCommand request,
            CancellationToken cancellationToken)
        {
            var budget =
                await _unitOfWork.Budgets.FindAsync(
                    x =>
                        x.Id == request.Id &&
                        x.UserId == _currentUserService.UserId,
                    cancellationToken);

            if (budget is null)
            {
                return Result<bool>.Failure(
                    "Budget not found.",
                    404);
            }

            _unitOfWork.Budgets.Remove(budget);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
