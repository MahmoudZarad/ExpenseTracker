using AutoMapper;
using ExpenseTracker.Application.Common.Patterns;
using ExpenseTracker.Application.DTOs.Budget;
using ExpenseTracker.Application.Features.Budgets.Queries.Models;
using ExpenseTracker.Application.Interfaces.Common;
using ExpenseTracker.Application.Interfaces.IUnitOfWork;
using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Entities;
using MediatR;

namespace ExpenseTracker.Application.Features.Budgets.Queries.Handlers
{
    public class GetBudgetsQueryHandler
    : IRequestHandler<
        GetBudgetsQuery,
        Result<PaginatedResult<BudgetDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;


        public GetBudgetsQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;   
        }

        public async Task<Result<PaginatedResult<BudgetDto>>> Handle(
            GetBudgetsQuery request,
            CancellationToken cancellationToken)
        {
            var options = new QueryOptions<Budget>
            {
                Filter = x => x.UserId == _currentUserService.UserId,

                OrderBy = x => x.Id,

                Descending = true,

                Skip = (request.Params.PageNumber - 1)
                       * request.Params.PageSize,

                Take = request.Params.PageSize,

                Tracking = false
            };

            var items =
                await _unitOfWork.Budgets
                    .GetAllAsync<BudgetDto>(
                        options,
                        _mapper.ConfigurationProvider,
                        cancellationToken);

            var totalCount =
                await _unitOfWork.Budgets.CountAsync(
                    x => x.UserId == _currentUserService.UserId,
                    cancellationToken);

            var result = new PaginatedResult<BudgetDto>
            {
                Items = items,
                PageNumber = request.Params.PageNumber,
                PageSize = request.Params.PageSize,
                TotalCount = totalCount
            };

            return Result<PaginatedResult<BudgetDto>>
                .Success(result);
        }
    }
}
