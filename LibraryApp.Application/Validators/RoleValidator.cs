using FluentValidation;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Validators;

public class RoleValidator : AbstractValidator<Role>
{
    public RoleValidator()
    {
        RuleFor(r => r.Id)
            .NotNull()
            .WithMessage("ID cannot be null.")
            .GreaterThan(0)
            .WithMessage("ID value must be greater than zero.")
            .When(r => r.Id != 0);

        RuleFor(r => r.Name)
            .NotEmpty()
            .WithMessage("Role name cannot be empty.")
            .Length(3,15)
            .WithMessage("Role name must be between 3-15 characters.");
    }
}