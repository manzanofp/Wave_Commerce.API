using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Wave.Commerce.Persistence.Context;

namespace Wave.Commerce.Persistence.StartupService;

public class EntityFrameworkCoreMigrator
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreMigrator(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public async Task ApplyMigrationsWithRetryAsync()
    {
        var logger = _serviceProvider.GetRequiredService<ILogger<EntityFrameworkCoreMigrator>>();
        var migrator = new EntityFrameworkCoreMigrator(_serviceProvider);

        const int maxRetryCount = 10;
        int retryCount = 0;
        bool dbReady = false;

        while (retryCount < maxRetryCount && !dbReady)
        {
            try
            {
                logger.LogInformation("Applying database migrations...");
                await migrator.Migrate();
                dbReady = true;
                logger.LogInformation("Database migrations applied successfully.");
            }
            catch (Exception ex)
            {
                retryCount++;
                logger.LogError(ex, $"Database migration failed. Retry {retryCount}/{maxRetryCount}");

                if (retryCount >= maxRetryCount)
                {
                    logger.LogError("Max retry count reached. Unable to apply database migrations.");
                    throw;
                }

                await Task.Delay(2000);
            }
        }
    }

    public async Task Migrate()
    {
        using var serviceScope = _serviceProvider.CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
    }
}
