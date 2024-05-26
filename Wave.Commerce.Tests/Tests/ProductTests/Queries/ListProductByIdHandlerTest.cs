using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Wave.Commerce.Application.Features.ProductFeatures.Queries.ListProductById;
using Wave.Commerce.Domain.Entities.ProductEntity;
using Wave.Commerce.Domain.Entities.ProductEntity.Repositories;
using Wave.Commerce.Tests.Shared;

namespace Wave.Commerce.Tests.Tests.ProductTests.Queries;

public class ListProductByIdHandlerTest
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<ILogger<ListProductByIdHandler>> _mockLogger;
    private readonly ListProductByIdHandler _handler;
    private readonly FakeRequests _query;
    private readonly FakeProduct _product;

    public ListProductByIdHandlerTest()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockLogger = new Mock<ILogger<ListProductByIdHandler>>();
        _handler = new ListProductByIdHandler(_mockProductRepository.Object, _mockLogger.Object);
        _query = new FakeRequests();
        _product = new FakeProduct();
    }

    [Fact]
    public async Task Handle_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var product = _product.CreateValidEntity();
        var query = _query.CreateValidListProductByIdQuery(product.Id);

        _mockProductRepository.Setup(repo => repo.GetById(product.Id))
            .ReturnsAsync(product);

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
    public async Task Handle_ShouldReturnError_WhenProductDoesNotExist()
    {
        // Arrange
        var product = _product.CreateValidEntity();
        var query = _query.CreateValidListProductByIdQuery(product.Id);

        _mockProductRepository.Setup(repo => repo.GetById(product.Id))
            .ReturnsAsync((Product)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be($"Product with Id:  {product.Id} not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenRepositoryThrowsException()
    {
        // Arrange
        var product = _product.CreateValidEntity();
        var query = _query.CreateValidListProductByIdQuery(product.Id);
        var exceptionMessage = "Database failure";

        _mockProductRepository.Setup(repo => repo.GetById(product.Id))
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be($"Error When trying to search product By Id: {exceptionMessage}");
    }
}
