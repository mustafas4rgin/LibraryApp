using FluentValidation;
using LibraryApp.Domain.DTOs.Update;

namespace LibraryApp.Application.DTOValidators.Update;

public class UpdateUserDTOValidator : AbstractValidator<UpdateUserDTO>
{
    public UpdateUserDTOValidator()
    {
        RuleFor(u => u.Address)
            .NotEmpty()
            .WithMessage("Address cannot be empty.")
            .Length(5 - 150)
            .WithMessage("Address should be between 5-150 characters.");

        RuleFor(u => u.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty.")
            .EmailAddress()
            .WithMessage("Must be a valid email address.")
            .Length(3, 50)
            .WithMessage("Email must be between 3-50 characters.");

        RuleFor(u => u.Name)
            .NotEmpty()
            .WithMessage("Name cannot be empty.")
            .Length(3, 50)
            .WithMessage("Name must be between 3-50 characters");

        RuleFor(u => u.Username)
            .NotEmpty()
            .WithMessage("Name cannot be empty.")
            .Length(3, 20)
            .WithMessage("Username should be between 3-20 characters.");
    }
}