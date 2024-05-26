using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using Wave.Commerce.IntegrationTests.Base;
using Wave.Commerce.Tests.Shared;

namespace Wave.Commerce.IntegrationTests.Tests.ProductFeatures;

[Collection(nameof(SharedTestCollection))]
public class UpdateProductFeaturesTest : IntegrationTestBase
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly FakeRequests _commands;
    private readonly FakeProduct _product;

    public UpdateProductFeaturesTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _factory = factory;
        _commands = new FakeRequests();
        _product = new FakeProduct();
    }

    [Fact]
    public async Task Should_Update_Product_When_Valid()
    {
        // Arrange
        var (context, _) = GetDbContext();

        var product = _product.CreateValidEntity();
        context.Products.Add(product);
        await context.SaveChangesAsync();

        var command = _commands.CreateValidUpdateCommand(product.Id, "New Name Product", product.Value, 300);

        // Act
        var response = await _factory.HttpClient
            .PutAsJsonAsync("/api/Product", command);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedProduct = await context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == product.Id);

        // Assert
        updatedProduct.Should().NotBeNull();
        updatedProduct!.Name.Should().Be(command.Name);
        updatedProduct.Value.Should().Be(command.Value);
        updatedProduct.StockQuantity.Should().Be(command.StockQuantity);

    }
}
