using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Interfaces.Common.Jwt
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
