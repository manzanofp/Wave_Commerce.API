using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using Wave.Commerce.IntegrationTests.Base;
using Wave.Commerce.Tests.Shared;

namespace Wave.Commerce.IntegrationTests.Tests.ProductFeatures;

[Collection(nameof(SharedTestCollection))]
public class InsertProductFeatureTest : IntegrationTestBase
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly FakeRequests _commands;

    public InsertProductFeatureTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _factory = factory;
        _commands = new FakeRequests();
    }

    [Fact]
    public async Task Should_Return_ProductId_When_Product_IsValid()
    {
        // Arrange
        var (context, _) = GetDbContext();

        var command = _commands.CreateValidInsertCommand();

        // Act
        var response = await _factory.HttpClient
            .PostAsJsonAsync("/api/Product", command);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();

        var productId = JsonConvert.DeserializeObject<Guid>(content);

        productId.Should().NotBeEmpty();

        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);

        // Assert
        product.Should().NotBeNull();
        product!.Name.Should().Be(command.Name);
        product.Value.Should().Be(command.Value);
        product.StockQuantity.Should().Be(command.StockQuantity);
    }
}
