using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Wave.Commerce.Domain.Entities.ProductEntity;
using Wave.Commerce.IntegrationTests.Base;

namespace Wave.Commerce.IntegrationTests.ProductFeatures;

[Collection(nameof(SharedTestCollection))]
public class DeleteProductFeatureTest : IntegrationTestBase
{
    private readonly CustomWebApplicationFactory _factory;

    public DeleteProductFeatureTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _factory = factory;
    }


    [Fact]
    public async Task Should_Delete_Product_When_Valid()
    {
        // Arrange
        var (context, _) = GetDbContext();

        var product = Product.CreateEntity(
            "Product for Delete",
            100.00m,
            20);

        context.Products.Add(product);
        await context.SaveChangesAsync();

        // Act
        var result = await _factory.HttpClient
            .DeleteAsync($"/api/Product/delete/{product.Id}");

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
