using FluentValidation;
using LibraryApp.Domain.DTOs.Update;

namespace LibraryApp.Application.DTOValidators.Update;

public class UpdateRoleDTOValidator : AbstractValidator<UpdateRoleDTO>
{
    public UpdateRoleDTOValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .WithMessage("Role name cannot be empty.")
            .Length(3,15)
            .WithMessage("Role name must be between 3-15 characters.");
    }
}