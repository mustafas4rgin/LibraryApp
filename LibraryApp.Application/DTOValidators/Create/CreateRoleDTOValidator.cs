using FluentValidation;
using LibraryApp.Domain.DTOs.Create;

namespace LibraryApp.Application.DTOValidators.Create;

public class CreateRoleDTOValidator : AbstractValidator<CreateRoleDTO>
{
    public CreateRoleDTOValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .WithMessage("Role name cannot be empty.")
            .Length(3,15)
            .WithMessage("Role name must be between 3-15 characters.");
    }
}