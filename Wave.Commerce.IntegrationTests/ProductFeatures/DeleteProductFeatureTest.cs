using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using Wave.Commerce.Domain.Entities.ProductEntity;
using Wave.Commerce.IntegrationTests.Base;
using Wave.Commerce.IntegrationTests.Shared;

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

    [Fact]
    public async Task Should_Return_ProblemDetails_When_Product_Dont_Exist()
    {
        // Arrange
        var nonExistingProductId = Guid.NewGuid();

        // Act
        var response = await _factory.HttpClient
            .DeleteAsync($"/api/Product/delete/{nonExistingProductId}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadAsStringAsync();

        var problemDetails = JsonConvert.DeserializeObject<ProblemDetailsDto>(content);

        //Assert
        problemDetails.Should().NotBeNull();
        problemDetails!.Title.Should().Be("Bad Request");
        problemDetails.Status.Should().Be(400);
        problemDetails.Detail.Should().Be($"Product with id:  {nonExistingProductId} not found");
    }
}
