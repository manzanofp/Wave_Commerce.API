using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using Wave.Commerce.Application.Features.ProductFeatures.Commands.UpdateProduct;
using Wave.Commerce.Domain.Entities.ProductEntity;
using Wave.Commerce.IntegrationTests.Base;

namespace Wave.Commerce.IntegrationTests.ProductFeatures;

[Collection(nameof(SharedTestCollection))]
public class UpdateProductFeaturesTest : IntegrationTestBase
{
    private readonly CustomWebApplicationFactory _factory;

    public UpdateProductFeaturesTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Should_Update_Product_When_Valid()
    {
        // Arrange
        var (context, _) = GetDbContext();

        var product = Product.CreateEntity("Product Name", 1299.99m, 50);
        context.Products.Add(product);
        await context.SaveChangesAsync();

        var command = new UpdateProductCommand(
            product.Id,
            "Product Name updated",
            1299.99m,   
            50);

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
