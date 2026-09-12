using ExpenseTracker.Application.Common.Patterns;
using ExpenseTracker.Application.Interfaces.Common;
using FluentValidation;

namespace ExpenseTracker.Application.Common.Validators;

public class PaginationValidator<T, TParams> : AbstractValidator<T> where T : IHasPagination<TParams> where TParams : PaginationParams

{
    public PaginationValidator()
    {
        RuleFor(x => x.Params.PageNumber)
            .GreaterThan(0);

        RuleFor(x => x.Params.PageSize)
            .InclusiveBetween(1, 50);
    }
}