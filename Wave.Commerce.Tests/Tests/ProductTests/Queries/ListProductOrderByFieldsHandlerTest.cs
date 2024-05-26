using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Wave.Commerce.Application.Features.ProductFeatures.Queries;
using Wave.Commerce.Application.Features.ProductFeatures.Queries.ListProductOrderByFields;
using Wave.Commerce.Domain.Entities.ProductEntity;
using Wave.Commerce.Domain.Entities.ProductEntity.Repositories;
using Wave.Commerce.Tests.Shared;

namespace Wave.Commerce.Tests.Tests.ProductTests.Queries;

public class ListProductOrderByFieldsHandlerTest
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<ILogger<ListProductOrderByFieldsHandler>> _mockLogger;
    private readonly ListProductOrderByFieldsHandler _handler;
    private readonly FakeRequests _query;
    private readonly FakeProduct _product;

    public ListProductOrderByFieldsHandlerTest()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockLogger = new Mock<ILogger<ListProductOrderByFieldsHandler>>();
        _handler = new ListProductOrderByFieldsHandler(_mockProductRepository.Object, _mockLogger.Object);
        _product = new FakeProduct();
        _query = new FakeRequests();
    }

    [Fact]
    public async Task Handle_ShouldReturnProducts_WhenProductsExistOrderedByName()
    {
        // Arrange
        var product = _product.CreateValidEntity();
        var query = _query.CreateValidListProductOrderByQuery("name", null);
        var products = _product.CreateProductList(product.Name);

        _mockProductRepository.Setup(repo => repo.GetOrderByFields(It.IsAny<string>(), It.IsAny<int?>()))
            .ReturnsAsync(products.OrderBy(p => p.Name).ToList());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(products.Count);

        var expectedResults = products.OrderBy(p => p.Name).Select(_product.MapToProductResult).ToList();
        result.Value.Should().BeEquivalentTo(expectedResults, options => options.ComparingByMembers<ProductResult>());
    }

    [Fact]
    public async Task Handle_ShouldReturnProductsOrderedByValue_WhenFieldIsValue()
    {
        // Arrange
        var product = _product.CreateValidEntity();
        var query = _query.CreateValidListProductOrderByQuery("value", null);
        var products = _product.CreateProductList(product.Name);

        _mockProductRepository.Setup(repo => repo.GetOrderByFields(It.IsAny<string>(), It.IsAny<int?>()))
            .ReturnsAsync(products.OrderByDescending(p => p.Value).ToList());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(products.Count);

        var expectedResults = products.OrderByDescending(p => p.Value).Select(_product.MapToProductResult).ToList();
        result.Value.Should().BeEquivalentTo(expectedResults, options => options.ComparingByMembers<ProductResult>());
    }

    [Fact]
    public async Task Handle_ShouldReturnProductsOrderedByStock_WhenFieldIsStock()
    {
        // Arrange
        var product = _product.CreateValidEntity();
        var query = _query.CreateValidListProductOrderByQuery("stock", null);
        var products = _product.CreateProductList(product.Name);

        _mockProductRepository.Setup(repo => repo.GetOrderByFields(It.IsAny<string>(), It.IsAny<int?>()))
            .ReturnsAsync(products.OrderByDescending(p => p.StockQuantity).ToList());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(products.Count);

        var expectedResults = products.OrderByDescending(p => p.StockQuantity).Select(_product.MapToProductResult).ToList();
        result.Value.Should().BeEquivalentTo(expectedResults, options => options.ComparingByMembers<ProductResult>());
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenProductsDoNotExist()
    {
        // Arrange
        _mockProductRepository.Setup(repo => repo.GetOrderByFields(It.IsAny<string>(), It.IsAny<int?>()))
            .ReturnsAsync((List<Product>)null);

        var query = _query.CreateValidListProductOrderByQuery("value", null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be($"Product order by field: value not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenRepositoryThrowsException()
    {
        // Arrange
        var exceptionMessage = "Database failure";

        _mockProductRepository.Setup(repo => repo.GetOrderByFields(It.IsAny<string>(), It.IsAny<int?>()))
            .ThrowsAsync(new Exception(exceptionMessage));


        var query = _query.CreateValidListProductOrderByQuery("stock", null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be($"Error When trying to List products Order By fields: {exceptionMessage}");
    }
}
