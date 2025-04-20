using FluentValidation;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Validators;

public class BookRentalValidator : AbstractValidator<BookRental>
{
    public BookRentalValidator()
    {
        RuleFor(br => br.BookId)
           .NotNull()
           .WithMessage("Book cannot be null.")
           .GreaterThan(0)
           .WithMessage("BookId value must be greater than zero.");

        RuleFor(br => br.UserId)
            .NotNull()
            .WithMessage("User cannot be null.")
            .GreaterThan(0)
            .WithMessage("UserId value must be greater than zero.");
    }
}