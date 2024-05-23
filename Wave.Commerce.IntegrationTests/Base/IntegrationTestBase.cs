using Microsoft.Extensions.DependencyInjection;
using Wave.Commerce.Persistence.Context;

namespace Wave.Commerce.IntegrationTests.Base;

public class IntegrationTestBase : IAsyncLifetime
{

    private readonly CustomWebApplicationFactory _factory;

    protected IntegrationTestBase(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    protected (ApplicationDbContext dbContext, IServiceScope scope) GetDbContext()
    {
        var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return (context, scope);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _factory.ResetDatabase();
}
