using ExpenseTracker.Application.DTOs.Budget;
using ExpenseTracker.Application.Features.Budgets.Queries.Models;
using ExpenseTracker.Application.Interfaces.Common;
using ExpenseTracker.Application.Interfaces.IUnitOfWork;
using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Budgets.Queries.Handlers
{
    public class GetBudgetSummaryQueryHandler
    : IRequestHandler<
        GetBudgetSummaryQuery,
        Result<List<BudgetSummaryDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetBudgetSummaryQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<List<BudgetSummaryDto>>> Handle(
            GetBudgetSummaryQuery request,
            CancellationToken cancellationToken)
        {
            var result =
                await _unitOfWork.Budgets.GetSummaryAsync(
                    _currentUserService.UserId,
                    cancellationToken);

            return Result<List<BudgetSummaryDto>>
                .Success(result);
        }
    }
}
