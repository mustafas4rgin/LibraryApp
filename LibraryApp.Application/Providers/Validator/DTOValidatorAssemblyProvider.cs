using LibraryApp.Application.DTOValidators.Create;
using LibraryApp.Application.DTOValidators.Update;
using LibraryApp.Application.Validators;

namespace LibraryApp.Application.Providers.Validator;

public class DTOValidatorAssemblyProvider
{
    public static Type[] GetValidatorAssemblies()
    {
        return new[]
        {
            typeof(CreateUserDTOValidator),
            typeof(CreateRoleDTOValidator),
            typeof(CreateBookDTOValidator),
            typeof(UpdateBookDTOValidator),
            typeof(UpdateRoleDTOValidator),
            typeof(UpdateUserDTOValidator),
            typeof(CreateBookRentalValidator),
            typeof(UpdateBookRentalValidator),
            typeof(UpdateUserDTOValidator)
        };
    }
}