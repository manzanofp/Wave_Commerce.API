using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Wave.Commerce.Application.Features.ProductFeatures.Queries;
using Wave.Commerce.Application.Features.ProductFeatures.Queries.ListProductByName;
using Wave.Commerce.Domain.Entities.ProductEntity;
using Wave.Commerce.Domain.Entities.ProductEntity.Repositories;

namespace Wave.Commerce.Tests.Tests.ProductTests.Queries;

public class ListProductByNameHandlerTest
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<ILogger<ListProductByNameHandler>> _mockLogger;
    private readonly ListProductByNameHandler _handler;
    private readonly Faker _faker;

    public ListProductByNameHandlerTest()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockLogger = new Mock<ILogger<ListProductByNameHandler>>();
        _handler = new ListProductByNameHandler(_mockProductRepository.Object, _mockLogger.Object);
        _faker = new Faker();
    }

    [Fact]
    public async Task Handle_ShouldReturnProducts_WhenProductsExist()
    {
        // Arrange
        var productName = _faker.Commerce.ProductName();
        var products = new List<Product>
        {
            Product.CreateEntity(
                productName,
                _faker.Random.Decimal(1, 1000),
                _faker.Random.Int(1, 100)
            ),
            Product.CreateEntity(
                productName,
                _faker.Random.Decimal(1, 1000),
                _faker.Random.Int(1, 100)
            )
        };

        _mockProductRepository.Setup(repo => repo.GetByName(productName))
            .ReturnsAsync(products);

        var query = new ListProductByNameQuery(productName);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(products.Count);

        var expectedResults = products.Select(MapToProductResult).ToList();
        result.Value.Should().BeEquivalentTo(expectedResults, options => options.ComparingByMembers<ProductResult>());
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenProductsDoNotExist()
    {
        // Arrange
        var productName = _faker.Commerce.ProductName();

        _mockProductRepository.Setup(repo => repo.GetByName(productName))
            .ReturnsAsync((List<Product>)null);

        var query = new ListProductByNameQuery(productName);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be($"Product with name:  {productName} not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenRepositoryThrowsException()
    {
        // Arrange
        var productName = _faker.Commerce.ProductName();
        var exceptionMessage = "Database failure";

        _mockProductRepository.Setup(repo => repo.GetByName(productName))
            .ThrowsAsync(new Exception(exceptionMessage));

        var query = new ListProductByNameQuery(productName);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be($"Error When trying to List products By Name: {exceptionMessage}");
    }

    private ProductResult MapToProductResult(Product product)
    {
        return new ProductResult(
            product.Id,
            product.Name,
            product.Value,
            product.StockQuantity
        );
    }
}
