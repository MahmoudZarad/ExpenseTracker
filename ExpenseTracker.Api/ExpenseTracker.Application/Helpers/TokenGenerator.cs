using System.Security.Cryptography;

namespace ExpenseTracker.Application.Helpers;

public static class TokenGenerator
{
    public static Task<string> Generate()
    {
        var bytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Task.FromResult(Convert.ToBase64String(bytes));
    }
}
