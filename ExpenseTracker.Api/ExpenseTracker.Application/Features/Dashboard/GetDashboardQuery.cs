using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Domain.Common;
using MediatR;

namespace ExpenseTracker.Application.Features.Dashboard
{
    public record GetDashboardQuery() : IRequest<Result<DashboardDto>>;
}
