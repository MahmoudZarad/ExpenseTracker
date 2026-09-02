using ExpenseTracker.Application.Features.Budgets.Commands.Models;
using FluentValidation;

namespace ExpenseTracker.Application.Features.Budgets.Validators
{
    public class UpdateBudgetCommandValidator
    : AbstractValidator<UpdateBudgetCommand>
    {
        public UpdateBudgetCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.CategoryId)
                .GreaterThan(0);

            RuleFor(x => x.Limit)
                .GreaterThan(0);
        }
    }
}
