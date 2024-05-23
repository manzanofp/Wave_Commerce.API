using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Wave.Commerce.IntegrationTests.Base;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ApplyMigrations<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        var serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<TContext>();

        context.Database.Migrate();

        return services;
    }
}
