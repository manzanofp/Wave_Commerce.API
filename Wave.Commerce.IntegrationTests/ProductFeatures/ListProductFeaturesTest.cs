using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using Wave.Commerce.Application.Features.ProductFeatures.Queries;
using Wave.Commerce.IntegrationTests.Base;

namespace Wave.Commerce.IntegrationTests.ProductFeatures;

[Collection(nameof(SharedTestCollection))]
public class ListProductFeaturesTest : IntegrationTestBase
{
    private readonly CustomWebApplicationFactory _factory;

    public ListProductFeaturesTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Should_Get_Ok_WithResults_If_Products_AreFound_ById()
    {
        // Arrange
        var productIdAlreadyExistInDatabase = "8270d170-d487-4a1b-8f0c-12eee8ca4a4a";

        var productComparison = ProductResult_WithDataExist_InDatabase_For_Comparison();

        // Act
        var response = await _factory.HttpClient.GetAsync($"/api/Product/{productIdAlreadyExistInDatabase}");

        var content = await response.Content.ReadAsStringAsync();
        var productResult = JsonConvert.DeserializeObject<ProductResult>(content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        productResult.Should().NotBeNull();
        productResult!.Id.Should().Be(productComparison.Id); 
        productResult.Name.Should().Be(productComparison.Name);
        productResult.Value.Should().Be(productComparison.Value);
        productResult.StockQuantity.Should().Be(productComparison.StockQuantity);
    }

    [Fact]
    public async Task Should_Get_Ok_WithResults_If_Products_AreFound_OrderByName()
    {
        // Arrange
        var nameSearch = "Apple Iphone 13";

        // Act
        var response = await _factory.HttpClient.GetAsync($"/api/Product/list/{nameSearch}");

        var content = await response.Content.ReadAsStringAsync();
        var listProductResult = JsonConvert.DeserializeObject<List<ProductResult>>(content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        listProductResult.Should().NotBeNull();
        listProductResult.Should().HaveCountGreaterThan(0);
        listProductResult!.First().Name.Should().Contain(nameSearch);
    }

    [Fact]
    public async Task Should_Get_Ok_WithResults_If_Products_AreFound_orderByValue()
    {
        // Arrange
        var field = "value";

        // Act
        var response = await _factory.HttpClient.GetAsync($"/api/Product/orderBy/{field}");

        var content = await response.Content.ReadAsStringAsync();
        var listProductResult = JsonConvert.DeserializeObject<List<ProductResult>>(content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        listProductResult.Should().NotBeNull();
        listProductResult.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task Should_Get_Ok_WithResults_If_Products_AreFound_orderByStockQuantity()
    {
        // Arrange
        var field = "stock";

        // Act
        var response = await _factory.HttpClient.GetAsync($"/api/Product/orderBy/{field}");

        var content = await response.Content.ReadAsStringAsync();
        var listProductResult = JsonConvert.DeserializeObject<List<ProductResult>>(content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        listProductResult.Should().NotBeNull();
        listProductResult.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task Should_Get_Ok_If_NoProducts_AreFound()
    {
        //Arrange
        var nameSearch = "fake product";

        // Act
       var response = await _factory.HttpClient.GetAsync($"/api/Product/list/{nameSearch}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.RequestMessage!.Content.Should().BeNull();
    }

    private static ProductResult ProductResult_WithDataExist_InDatabase_For_Comparison()
    {
        return new ProductResult(Guid.Parse("8270d170-d487-4a1b-8f0c-12eee8ca4a4a"), "Dell XPS 13 Laptop", 999.80m, 150);
    }

    private static List<ProductResult> ListProductResults_WithDataExists_InDatabase_For_Comparison()
    {
        var products = new List<ProductResult>
        {
            new(Guid.Parse("076d553e-4bf8-4367-931a-e7742501e8b6"), "Apple Iphone 13", 799.99m, 50),
            new(Guid.Parse("3c253a59-f32c-4ff1-9ab7-af245e50e291"), "Samsung Galaxy S21", 699.99m, 70),
            new(Guid.Parse("8270d170-d487-4a1b-8f0c-12eee8ca4a4a"), "Dell XPS 13 Laptop", 999.80m, 150),
            new(Guid.Parse("02664afa-6ab9-4ef7-9619-b3644f9c02e4"), "Amazon Echo Dot (4th Gen)", 60.50m, 350),
            new(Guid.Parse("7bec3fbb-94cb-4d09-976b-d0831b3b9ad8"), "Galaxy Watch pro 5", 1260.50m, 350)
        };

        return products;
    }
}
