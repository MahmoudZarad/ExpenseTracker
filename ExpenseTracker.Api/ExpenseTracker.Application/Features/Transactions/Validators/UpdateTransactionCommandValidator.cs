using FluentValidation;
using ExpenseTracker.Application.Features.Transactions.Commands.Models;
using ExpenseTracker.Application.Features.Categories.Commands.Models;

namespace ExpenseTracker.Application.Features.Transactions.Validators;

public class UpdateTransactionCommandValidator
    : AbstractValidator<UpdateTransactionCommand>
{
    public UpdateTransactionCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .MaximumLength(500);

        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.CategoryId)
            .GreaterThan(0);
    }
}
