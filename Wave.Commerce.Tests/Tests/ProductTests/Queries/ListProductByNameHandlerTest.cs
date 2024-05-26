using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Wave.Commerce.Application.Features.ProductFeatures.Queries;
using Wave.Commerce.Application.Features.ProductFeatures.Queries.ListProductByName;
using Wave.Commerce.Domain.Entities.ProductEntity;
using Wave.Commerce.Domain.Entities.ProductEntity.Repositories;
using Wave.Commerce.Tests.Shared;

namespace Wave.Commerce.Tests.Tests.ProductTests.Queries;

public class ListProductByNameHandlerTest
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<ILogger<ListProductByNameHandler>> _mockLogger;
    private readonly ListProductByNameHandler _handler;
    private readonly FakeRequests _query;
    private readonly FakeProduct _product;

    public ListProductByNameHandlerTest()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockLogger = new Mock<ILogger<ListProductByNameHandler>>();
        _handler = new ListProductByNameHandler(_mockProductRepository.Object, _mockLogger.Object);
        _query = new FakeRequests();
        _product = new FakeProduct();
    }

    [Fact]
    public async Task Handle_ShouldReturnProducts_WhenProductsExist()
    {
        // Arrange
        var product = _product.CreateValidEntity();
        var query = _query.CreateValidListProductByNameQuery(product.Name);
        var products = _product.CreateProductList(product.Name);

        _mockProductRepository.Setup(repo => repo.GetByName(product.Name))
            .ReturnsAsync(products);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(products.Count);

        var expectedResults = products.Select(_product.MapToProductResult).ToList();
        result.Value.Should().BeEquivalentTo(expectedResults, options => options.ComparingByMembers<ProductResult>());
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenProductsDoNotExist()
    {
        // Arrange
        var product = _product.CreateValidEntity();
        var query = _query.CreateValidListProductByNameQuery(product.Name);

        _mockProductRepository.Setup(repo => repo.GetByName(product.Name))
            .ReturnsAsync((List<Product>)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be($"Product with name:  {product.Name} not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenRepositoryThrowsException()
    {
        // Arrange
        var product = _product.CreateValidEntity();
        var query = _query.CreateValidListProductByNameQuery(product.Name);
        var exceptionMessage = "Database failure";

        _mockProductRepository.Setup(repo => repo.GetByName(product.Name))
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be($"Error When trying to List products By Name: {exceptionMessage}");
    }
}
