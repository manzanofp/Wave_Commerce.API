using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Net;
using Wave.Commerce.Application.Features.ProductFeatures.Commands.InsertProduct;
using Wave.Commerce.IntegrationTests.Base;
using FluentAssertions;

namespace Wave.Commerce.IntegrationTests.ProductFeatures;

[Collection(nameof(SharedTestCollection))]
public class InsertProductFeatureTest : IntegrationTestBase
{
    private readonly CustomWebApplicationFactory _factory;

    public InsertProductFeatureTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _factory = factory;
    }


    [Fact]
    public async Task Should_Return_ProductId_When_Product_IsValid()
    {
        // Arrange
        var (context, _) = GetDbContext();

        var command = new InsertProductCommand(
            "Product Name",
            1299.99m,
            50);

        // Act
        var response = await _factory.HttpClient
            .PostAsJsonAsync("/api/Product", command);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();

        var productId = JsonConvert.DeserializeObject<Guid>(content);

        productId.Should().NotBeEmpty();

        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);

        product.Should().NotBeNull();
        product!.Name.Should().Be(command.Name);
        product.Value.Should().Be(command.Value);
        product.StockQuantity.Should().Be(command.StockQuantity);
    }
}
