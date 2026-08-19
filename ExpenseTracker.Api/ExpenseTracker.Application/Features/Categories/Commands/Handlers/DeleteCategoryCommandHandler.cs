using ExpenseTracker.Application.Features.Categories.Commands.Models;
using ExpenseTracker.Application.Interfaces.Common;
using ExpenseTracker.Application.Interfaces.IUnitOfWork;
using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Categories.Commands.Handlers
{
    public class DeleteCategoryCommandHandler
    : IRequestHandler<
        DeleteCategoryCommand,
        Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeleteCategoryCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(
            DeleteCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var category =
                await _unitOfWork.Categories.FindAsync(
                    x =>
                        x.Id == request.Id &&
                        x.UserId == _currentUserService.UserId,
                    cancellationToken);

            if (category is null)
            {
                return Result<bool>.Failure(
                    "Category not found.",
                    404);
            }

            var hasTransactions =
    await _unitOfWork.Transactions.AnyAsync(
        x => x.CategoryId == request.Id,
        cancellationToken);

            if (hasTransactions)
            {
                return Result<bool>.Failure(
                    "Cannot delete a category that has transactions.",
                    409);
            }


            var hasBudget =
                await _unitOfWork.Budgets.AnyAsync(
                    x => x.CategoryId == request.Id,
                    cancellationToken);

            if (hasBudget)
            {
                return Result<bool>.Failure(
                    "Cannot delete a category that has a budget.",
                    409);
            }

            _unitOfWork.Categories.Remove(category);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
