using FluentValidation;
using LibraryApp.Domain.DTOs.Create;

namespace LibraryApp.Application.DTOValidators.Create;

public class CreateUserDTOValidator : AbstractValidator<CreateUserDTO>
{
    public CreateUserDTOValidator()
    {
        RuleFor(u => u.Address)
            .NotEmpty()
            .WithMessage("Address cannot be empty.")
            .Length(5 , 150)
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

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre zorunludur.")
            .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalı.")
            .Matches(@"[A-Z]+").WithMessage("Şifre en az bir büyük harf içermeli.")
            .Matches(@"[a-z]+").WithMessage("Şifre en az bir küçük harf içermeli.")
            .Matches(@"\d+").WithMessage("Şifre en az bir rakam içermeli.")
            .Matches(@"[\!\@\#\$\%\^\&\*\(\)\-\+\=]+").WithMessage("Şifre en az bir özel karakter içermeli.");
    }
}