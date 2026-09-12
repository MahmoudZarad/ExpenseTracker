using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Interfaces.Common;
using ExpenseTracker.Application.Interfaces.IRepositories.IEntitiesRepository;
using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Dashboard;

public class GetDashboardQueryHandler(
    IDashboardRepository repository, ICurrentUserService currentUserService)
    : IRequestHandler<
        GetDashboardQuery,
        Result<DashboardDto>>
{
    public async Task<Result<DashboardDto>> Handle(
        GetDashboardQuery request,
        CancellationToken cancellationToken)
    {
        var dashboard =
            await repository.GetDashboardAsync(
                currentUserService.UserId, cancellationToken);

        return Result<DashboardDto>.Success(
            dashboard);
    }
}