using FluentValidation;
using LibraryApp.Domain.DTOs.Update;

namespace LibraryApp.Application.DTOValidators.Update;

public class UpdateUsersRoleDTOValidator : AbstractValidator<UpdateUsersRoleDTO>
{
    public UpdateUsersRoleDTOValidator()
    {
        RuleFor(uur => uur.RoleId)
            .NotNull()
            .WithMessage("Role ID cannot be null.")
            .GreaterThan(0)
            .WithMessage("Role ID value must be greater than zero.");

        RuleFor(uur => uur.UserId)
            .NotNull()
            .WithMessage("User ID cannot be null.")
            .GreaterThan(0)
            .WithMessage("User ID value cannot be null.");
    }
}