
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wave.Commerce.Persistence;
using Wave.Commerce.Persistence.Context;

namespace DependencyInjection;

public static class Injector
{
    public static IServiceCollection AddService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        AddPersistenceServices(services, configuration);

        return services;
    }

    private static void AddPersistenceServices(IServiceCollection services, IConfiguration configuration)
    {
        string? persistenceAssemblyName = typeof(AssemblyReference).Assembly.GetName().Name;

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

        services.AddDbContext<ApplicationDbContext>();
    }
}
