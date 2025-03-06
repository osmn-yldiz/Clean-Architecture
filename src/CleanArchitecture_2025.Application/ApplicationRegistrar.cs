using CleanArchitecture_2025.Application.Behaviors;
using CleanArchitecture_2025.Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture_2025.Application;

public static class ApplicationRegistrar
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblies(
                typeof(ApplicationRegistrar).Assembly,
                typeof(AppUser).Assembly); ;
            configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(ApplicationRegistrar).Assembly);

        return services;

    }
}
