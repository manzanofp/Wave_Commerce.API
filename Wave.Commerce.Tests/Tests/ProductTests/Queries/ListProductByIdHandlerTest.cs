using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Wave.Commerce.Application.Features.ProductFeatures.Queries.ListProductById;
using Wave.Commerce.Domain.Entities.ProductEntity;
using Wave.Commerce.Domain.Entities.ProductEntity.Repositories;

namespace Wave.Commerce.Tests.Tests.ProductTests.Queries;

public class ListProductByIdHandlerTest
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<ILogger<ListProductByIdHandler>> _mockLogger;
    private readonly ListProductByIdHandler _handler;
    private readonly Faker _faker;

    public ListProductByIdHandlerTest()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockLogger = new Mock<ILogger<ListProductByIdHandler>>();
        _handler = new ListProductByIdHandler(_mockProductRepository.Object, _mockLogger.Object);
        _faker = new Faker();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Handle_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var product = Product.CreateEntity(
            _faker.Commerce.ProductName(),
            _faker.Random.Decimal(1, 1000),
            _faker.Random.Int(1, 100));

        _mockProductRepository.Setup(repo => repo.GetById(product.Id))
            .ReturnsAsync(product);

        var query = new ListProductByIdQuery(product.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be(product.Id);
        result.Value.Name.Should().Be(product.Name);
        result.Value.Value.Should().Be(product.Value);
        result.Value.StockQuantity.Should().Be(product.StockQuantity);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Handle_ShouldReturnError_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = _faker.Random.Guid();

        _mockProductRepository.Setup(repo => repo.GetById(productId))
            .ReturnsAsync((Product)null);

        var query = new ListProductByIdQuery(productId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be($"Product with Id:  {productId} not found.");
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Handle_ShouldReturnError_WhenRepositoryThrowsException()
    {
        // Arrange
        var productId = _faker.Random.Guid();
        var exceptionMessage = "Database failure";

        _mockProductRepository.Setup(repo => repo.GetById(productId))
            .ThrowsAsync(new Exception(exceptionMessage));

        var query = new ListProductByIdQuery (productId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be($"Error When trying to search product By Id: {exceptionMessage}");
    }
}
