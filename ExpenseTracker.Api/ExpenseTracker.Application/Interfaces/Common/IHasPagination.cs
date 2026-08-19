using ExpenseTracker.Application.Common.Patterns;

namespace ExpenseTracker.Application.Interfaces.Common;

public interface IHasPagination<T> where T : class
{
    T Params { get; }
}
