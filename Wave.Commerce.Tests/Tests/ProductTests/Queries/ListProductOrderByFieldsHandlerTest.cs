using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Wave.Commerce.Application.Features.ProductFeatures.Queries;
using Wave.Commerce.Application.Features.ProductFeatures.Queries.ListProductOrderByFields;
using Wave.Commerce.Application.Features.ProductFeatures.Queries.ListProductOrderedBy;
using Wave.Commerce.Domain.Entities.ProductEntity;
using Wave.Commerce.Domain.Entities.ProductEntity.Repositories;

namespace Wave.Commerce.Tests.Tests.ProductTests.Queries;

public class ListProductOrderByFieldsHandlerTest
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<ILogger<ListProductOrderByFieldsHandler>> _mockLogger;
    private readonly ListProductOrderByFieldsHandler _handler;
    private readonly Faker _faker;

    public ListProductOrderByFieldsHandlerTest()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockLogger = new Mock<ILogger<ListProductOrderByFieldsHandler>>();
        _handler = new ListProductOrderByFieldsHandler(_mockProductRepository.Object, _mockLogger.Object);
        _faker = new Faker();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Handle_ShouldReturnProducts_WhenProductsExistOrderedByName()
    {
        var products = new List<Product>
        {
            Product.CreateEntity(
                _faker.Commerce.ProductName(),
                _faker.Random.Decimal(1, 1000),
                _faker.Random.Int(1, 100)
            ),
            Product.CreateEntity(
                _faker.Commerce.ProductName(),
                _faker.Random.Decimal(1, 1000),
                _faker.Random.Int(1, 100)
            )
        };

        // Arrange
        _mockProductRepository.Setup(repo => repo.GetOrderByFields(It.IsAny<string>(), It.IsAny<int?>()))
            .ReturnsAsync(products.OrderBy(p => p.Name).ToList());

        var query = new ListProductOrderByFieldsQuery ("name", null );

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(products.Count);

        var expectedResults = products.OrderBy(p => p.Name).Select(MapToProductResult).ToList();
        result.Value.Should().BeEquivalentTo(expectedResults, options => options.ComparingByMembers<ProductResult>());
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Handle_ShouldReturnProductsOrderedByValue_WhenFieldIsValue()
    {
        // Arrange
        var products = new List<Product>
        {
            Product.CreateEntity(
                _faker.Commerce.ProductName(),
                500m,
                _faker.Random.Int(1, 100)
            ),
            Product.CreateEntity(
                _faker.Commerce.ProductName(),
                1000m,
                _faker.Random.Int(1, 100)
            ),
            Product.CreateEntity(
                _faker.Commerce.ProductName(),
                1500m,
                _faker.Random.Int(1, 100)
            )
        };

        _mockProductRepository.Setup(repo => repo.GetOrderByFields(It.IsAny<string>(), It.IsAny<int?>()))
            .ReturnsAsync(products.OrderByDescending(p => p.Value).ToList());

        var query = new ListProductOrderByFieldsQuery("value", null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(products.Count);

        var expectedResults = products.OrderByDescending(p => p.Value).Select(MapToProductResult).ToList();
        result.Value.Should().BeEquivalentTo(expectedResults, options => options.ComparingByMembers<ProductResult>());
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Handle_ShouldReturnProductsOrderedByStock_WhenFieldIsStockAndStockValueIsNull()
    {
        // Arrange
        var products = new List<Product>
        {
            Product.CreateEntity(
                _faker.Commerce.ProductName(),
                _faker.Random.Decimal(1, 1000),
                10
            ),
            Product.CreateEntity(
                _faker.Commerce.ProductName(),
                _faker.Random.Decimal(1, 1000),
                30
            ),
            Product.CreateEntity(
                _faker.Commerce.ProductName(),
                _faker.Random.Decimal(1, 1000),
                20
            )
        };

        _mockProductRepository.Setup(repo => repo.GetOrderByFields(It.IsAny<string>(), It.IsAny<int?>()))
            .ReturnsAsync(products.OrderByDescending(p => p.StockQuantity).ToList());

        var query = new ListProductOrderByFieldsQuery ("stock", 2);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(products.Count);

        var expectedResults = products.OrderByDescending(p => p.StockQuantity).Select(MapToProductResult).ToList();
        result.Value.Should().BeEquivalentTo(expectedResults, options => options.ComparingByMembers<ProductResult>());
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Handle_ShouldReturnError_WhenProductsDoNotExist()
    {
        // Arrange
        _mockProductRepository.Setup(repo => repo.GetOrderByFields(It.IsAny<string>(), It.IsAny<int?>()))
            .ReturnsAsync((List<Product>)null);

        var query = new ListProductOrderByFieldsQuery("stock", 2);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be($"Product order by field: stock not found.");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Handle_ShouldReturnError_WhenRepositoryThrowsException()
    {
        // Arrange
        var exceptionMessage = "Database failure";

        _mockProductRepository.Setup(repo => repo.GetOrderByFields(It.IsAny<string>(), It.IsAny<int?>()))
            .ThrowsAsync(new Exception(exceptionMessage));

        var query = new ListProductOrderByFieldsQuery("value", 2);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be($"Error When trying to List products Order By fields: {exceptionMessage}");
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
