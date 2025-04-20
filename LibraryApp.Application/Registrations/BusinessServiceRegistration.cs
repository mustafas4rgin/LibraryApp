using LibraryApp.Application.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryApp.Application.Registrations;

public static class BusinessServiceRegistration
{
    public static IServiceCollection AddBusinessService(this IServiceCollection services)
    {
        ServiceRegistrationProvider.RegisterServices(services);

        return services;
    }
}