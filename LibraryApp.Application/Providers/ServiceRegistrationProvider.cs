using LibraryApp.Application.Concrete;
using LibraryApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryApp.Application.Providers;

public class ServiceRegistrationProvider
{
    public static void RegisterServices(IServiceCollection services)
    {
        var servicesToRegister = new (Type Interface, Type Implementation)[]
        {
            (typeof(IGenericService<>),typeof(GenericService<>)),
            (typeof(IUserService),typeof(UserService)),
            (typeof(IBookService),typeof(BookService)),
            (typeof(IBookRentalService),typeof(BookRentalService)),
            (typeof(IAdminService),typeof(AdminService)),
            (typeof(IAuthService),typeof(AuthService))
        };
        foreach (var service in servicesToRegister)
        {
            services.AddTransient(service.Interface, service.Implementation);
        }
    }
}