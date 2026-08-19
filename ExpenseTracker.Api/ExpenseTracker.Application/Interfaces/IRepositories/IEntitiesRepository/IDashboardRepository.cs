using ExpenseTracker.Application.DTOs;

namespace ExpenseTracker.Application.Interfaces.IRepositories.IEntitiesRepository
{
    public interface IDashboardRepository
    {
        Task<DashboardDto> GetDashboardAsync(
            int userId,
            CancellationToken cancellationToken = default);
    }
}
