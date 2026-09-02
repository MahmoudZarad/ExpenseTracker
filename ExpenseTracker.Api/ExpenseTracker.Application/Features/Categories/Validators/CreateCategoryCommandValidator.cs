using ExpenseTracker.Application.Features.Categories.Commands.Models;
using FluentValidation;

namespace ExpenseTracker.Application.Features.Categories.Validators;

public class CreateCategoryCommandValidator
    : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Type)
            .IsInEnum();
    }
}