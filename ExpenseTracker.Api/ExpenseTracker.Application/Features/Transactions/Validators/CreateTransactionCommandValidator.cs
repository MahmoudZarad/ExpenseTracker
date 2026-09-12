using ExpenseTracker.Application.Features.Transactions.Commands.Models;
using FluentValidation;

namespace ExpenseTracker.Application.Features.Transactions.Validators;

public class CreateTransactionCommandValidator
    : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .MaximumLength(500);

        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.Date)
            .NotEmpty();

        RuleFor(x => x.CategoryId)
            .GreaterThan(0);
    }
}
