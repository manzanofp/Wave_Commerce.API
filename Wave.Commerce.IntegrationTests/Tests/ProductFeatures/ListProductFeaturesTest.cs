using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using Wave.Commerce.Application.Features.ProductFeatures.Queries;
using Wave.Commerce.Domain.Entities.ProductEntity;
using Wave.Commerce.IntegrationTests.Base;
using Wave.Commerce.Persistence.Context;
using FakeProduct = Wave.Commerce.IntegrationTests.Shared.FakeProduct;

namespace Wave.Commerce.IntegrationTests.Tests.ProductFeatures;

[Collection(nameof(SharedTestCollection))]
public class ListProductFeaturesTest : IntegrationTestBase
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly List<Product> _fakeListProducts;
    private readonly ProductResult _fakeProductResult;
    private readonly ApplicationDbContext _dbContext;

    public ListProductFeaturesTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _factory = factory;
        _fakeListProducts = FakeProduct.FakeListProduct();
        _fakeProductResult = FakeProduct.FakeProductResult();

        var (_dbContext, _) = GetDbContext();
        this._dbContext = _dbContext;
        _dbContext.Products.AddRange(_fakeListProducts);
        _dbContext.SaveChangesAsync();

    }

    [Fact]
    public async Task Should_Get_Ok_WithResults_If_Products_AreFound_ById()
    {
        // Arrange
        var productIdAlreadyExistInDatabase = Guid.Parse("8270d170-d487-4a1b-8f0c-12eee8ca4a4a");

        var productResultComparison = _fakeProductResult;

        // Act
        var response = await _factory.HttpClient.GetAsync($"/api/Product/{productIdAlreadyExistInDatabase}");

        var content = await response.Content.ReadAsStringAsync();
        var productResult = JsonConvert.DeserializeObject<ProductResult>(content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        productResult.Should().NotBeNull();
        productResult!.Id.Should().Be(productResultComparison.Id);
        productResult.Name.Should().Be(productResultComparison.Name);
        productResult.Value.Should().Be(productResultComparison.Value);
        productResult.StockQuantity.Should().Be(productResultComparison.StockQuantity);
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
}
