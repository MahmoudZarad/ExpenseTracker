using AutoMapper;
using ExpenseTracker.Application.Features.Budgets.Commands.Models;
using ExpenseTracker.Application.Interfaces.Common;
using ExpenseTracker.Application.Interfaces.IUnitOfWork;
using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Enums;
using MediatR;

namespace ExpenseTracker.Application.Features.Budgets.Commands.Handlers
{
    public class UpdateBudgetCommandHandler
    : IRequestHandler<
        UpdateBudgetCommand,
        Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public UpdateBudgetCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(
            UpdateBudgetCommand request,
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

            if (category.Type != TransactionType.Expense)
            {
                return Result<bool>.Failure(
                    "Budgets can only be assigned to expense categories.",
                    400);
            }

            var exists =
                await _unitOfWork.Budgets.AnyAsync(
                    x =>
                        x.Id != request.Id &&
                        x.CategoryId == request.CategoryId &&
                        x.UserId == _currentUserService.UserId,
                    cancellationToken);

            if (exists)
            {
                return Result<bool>.Failure(
                    "A budget already exists for this category.",
                    409);
            }

            _mapper.Map(request, budget);

            await _unitOfWork.Budgets.UpdateAsync(budget);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
