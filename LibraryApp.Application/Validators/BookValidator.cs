using System.IO.Compression;
using FluentValidation;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Validators;

public class BookValidator : AbstractValidator<Book>
{
    public BookValidator()
    {
        RuleFor(b => b.Id)
            .NotNull()
            .WithMessage("ID value cannot be null.")
            .GreaterThan(0)
            .WithMessage("ID value must be greater than zero.")
            .When(b => b.Id != 0);

        RuleFor(b => b.ISBN)
            .NotEmpty()
            .WithMessage("ISBN number cannot be empty.")
            .Length(13)
            .WithMessage("ISBN must be 13 characters.");

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