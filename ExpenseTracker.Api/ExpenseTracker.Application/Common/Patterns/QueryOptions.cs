using System.Linq.Expressions;

namespace ExpenseTracker.Application.Common.Patterns;

public class QueryOptions<T>
{
    public Expression<Func<T, bool>>? Filter { get; init; }

    public Expression<Func<T, object>>? OrderBy { get; init; }

    public bool Descending { get; init; }

    public int? Skip { get; init; }

    public int? Take { get; init; }

    public bool Tracking { get; init; } = false;
}