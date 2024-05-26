using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using Wave.Commerce.IntegrationTests.Base;
using Wave.Commerce.IntegrationTests.Shared;
using FakeProduct = Wave.Commerce.Tests.Shared.FakeProduct;

namespace Wave.Commerce.IntegrationTests.Tests.ProductFeatures;

[Collection(nameof(SharedTestCollection))]
public class DeleteProductFeatureTest : IntegrationTestBase
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly FakeProduct _product;

    public DeleteProductFeatureTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _factory = factory;
        _product = new FakeProduct();
    }

    [Fact]
    public async Task Should_Delete_Product_When_Valid()
    {
        // Arrange
        var (context, _) = GetDbContext();

        var product = _product.CreateValidEntity();

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
