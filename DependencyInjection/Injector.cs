
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Wave.Commerce.Domain.Entities.ProductEntity.Repositories;
using Wave.Commerce.Persistence.Context;
using Wave.Commerce.Persistence.Repositories;

namespace DependencyInjection;

public static class Injector
{
    public static IServiceCollection AddServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        AddPersistenceServices(services, configuration);
        AddApplicationServices(services);

        return services;
    }

    private static void AddApplicationServices(IServiceCollection services)
    {
        Assembly applicationAssembly = typeof(Wave.Commerce.Application.AssemblyReference).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Wave.Commerce.Application.AssemblyReference).GetTypeInfo().Assembly));
        services.AddScoped<IMediator, Mediator>();
        services.AddValidatorsFromAssembly(applicationAssembly, includeInternalTypes: true);
    }

    private static void AddPersistenceServices(IServiceCollection services, IConfiguration configuration)
    {
        string? persistenceAssemblyName = typeof(Wave.Commerce.Persistence.AssemblyReference).Assembly.GetName().Name;

        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.ConfigureWarnings(builder =>
            {
                builder.Ignore(CoreEventId.PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning);
            });

            opt.UseNpgsql(configuration.GetConnectionString("DatabaseConnection"), builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(20), null);
            });
        });

        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddDbContext<ApplicationDbContext>();
    }
}
