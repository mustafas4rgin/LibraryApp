using LibraryApp.Application.Validators;

namespace LibraryApp.Application.Providers.Validator;

public class ValidatorAssemblyProvider
{
    public static Type[] GetValidatorAssemblies()
    {
        return new[]
        {
            typeof(UserValidator),
            typeof(RoleValidator),
            typeof(BookValidator),
            typeof(BookRentalValidator)
        };
    }
}