using AutoMapper;
using ExpenseTracker.Application.Features.Budgets.Commands.Models;
using ExpenseTracker.Application.Interfaces.Common;
using ExpenseTracker.Application.Interfaces.IUnitOfWork;
using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Enums;
using MediatR;

namespace ExpenseTracker.Application.Features.Budgets.Commands.Handlers
{
    public class CreateBudgetCommandHandler
    : IRequestHandler<CreateBudgetCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CreateBudgetCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(
            CreateBudgetCommand request,
            CancellationToken cancellationToken)
        {
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
                return Result<int>.Failure(
                    "Category not found.",
                    404);
            }

            if (category.Type != TransactionType.Expense)
            {
                return Result<int>.Failure(
                    "Budgets can only be created for expense categories.",
                    400);
            }

            var exists =
                await _unitOfWork.Budgets.AnyAsync(
                    x =>
                        x.CategoryId == request.CategoryId &&
                        x.UserId == _currentUserService.UserId,
                    cancellationToken);

            if (exists)
                return Result<int>.Failure("A budget already exists for this category.", 409);

            var budget = _mapper.Map<Budget>(request);
            budget.UserId = _currentUserService.UserId;

            await _unitOfWork.Budgets.AddAsync(
                budget,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return Result<int>.Success(budget.Id);
        }
    }
}
