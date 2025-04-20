using FluentValidation;
using LibraryApp.Domain.DTOs.Update;

namespace LibraryApp.Application.DTOValidators.Update;

public class UpdateBookRentalValidator : AbstractValidator<UpdateBookRental>
{
    public UpdateBookRentalValidator()
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