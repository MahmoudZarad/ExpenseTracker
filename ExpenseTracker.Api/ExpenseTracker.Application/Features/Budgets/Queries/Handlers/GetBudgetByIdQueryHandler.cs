using AutoMapper;
using ExpenseTracker.Application.DTOs.Budget;
using ExpenseTracker.Application.Features.Budgets.Queries.Models;
using ExpenseTracker.Application.Interfaces.Common;
using ExpenseTracker.Application.Interfaces.IUnitOfWork;
using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Budgets.Queries.Handlers
{
    public class GetBudgetByIdQueryHandler
    : IRequestHandler<
        GetBudgetByIdQuery,
        Result<BudgetDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetBudgetByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result<BudgetDto>> Handle(
            GetBudgetByIdQuery request,
            CancellationToken cancellationToken)
        {
            var budget =
                await _unitOfWork.Budgets.GetByAsync<BudgetDto>(
                    x =>
                        x.Id == request.Id &&
                        x.UserId == _currentUserService.UserId,

                    _mapper.ConfigurationProvider,

                    cancellationToken);

            if (budget is null)
            {
                return Result<BudgetDto>.Failure(
                    "Budget not found.",
                    404);
            }

            return Result<BudgetDto>.Success(budget);
        }
    }
}
