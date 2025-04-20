using FluentValidation;
using LibraryApp.Domain.DTos.Create;

namespace LibraryApp.Application.DTOValidators.Create;

public class CreateBookDTOValidator : AbstractValidator<CreateBookDTO>
{
    public CreateBookDTOValidator()
    {
        RuleFor(b => b.Page)
            .NotEmpty()
            .WithMessage("Page number cannot be empty.");

        RuleFor(b => b.Title)
            .NotEmpty()
            .WithMessage("Title cannot be empty.")
            .Length(2,40)
            .WithMessage("Title must be between 2-40 characters.");

        RuleFor(b => b.Stock)
            .NotNull()
            .WithMessage("Stock cannot be null.")
            .GreaterThan(0)
            .WithMessage("Stock must be greater than zero.");

        RuleFor(b => b.Author)
            .NotEmpty()
            .WithMessage("Author cannot be null.")
            .Length(2,20)
            .WithMessage("Author should be between 2-20 characters.");
    }
}