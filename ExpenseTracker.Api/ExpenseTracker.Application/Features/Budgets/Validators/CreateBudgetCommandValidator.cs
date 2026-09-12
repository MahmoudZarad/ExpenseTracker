using ExpenseTracker.Application.Features.Budgets.Commands.Models;
using FluentValidation;

namespace ExpenseTracker.Application.Features.Budgets.Validators;

public class CreateBudgetCommandValidator
: AbstractValidator<CreateBudgetCommand>
{
    public CreateBudgetCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .GreaterThan(0);

        RuleFor(x => x.Limit)
            .GreaterThan(0);
    }
}
