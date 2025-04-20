using FluentValidation;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.Username)
            .NotEmpty()
            .WithMessage("Name cannot be empty.")
            .Length(3,20)
            .WithMessage("Username should be between 3-20 characters.");

        RuleFor(u => u.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty.")
            .EmailAddress()
            .WithMessage("Must be a valid email address.")
            .Length(3,50)
            .WithMessage("Email must be between 3-50 characters.");

        RuleFor(u => u.Name)
            .NotEmpty()
            .WithMessage("Name cannot be empty.")
            .Length(3,50)
            .WithMessage("Name must be between 3-50 characters");

        RuleFor(u => u.Address)
            .NotEmpty()
            .WithErrorCode("Address cannot be empty.")
            .Length(10,150)
            .WithMessage("Address must be between 10-150 characters.");

        RuleFor(u => u.Id)
            .NotNull()
            .WithMessage("ID cannot be null.")
            .GreaterThan(0)
            .WithMessage("ID value must be greater than zero.");

        RuleFor(u => u.RoleId)
           .NotNull()
           .WithMessage("ID cannot be null.")
           .GreaterThan(0)
           .WithMessage("ID value must be greater than zero.");
    }
}