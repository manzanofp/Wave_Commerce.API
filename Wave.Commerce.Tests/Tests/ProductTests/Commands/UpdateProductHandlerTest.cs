using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel;
using Wave.Commerce.Application.Features.ProductFeatures.Commands.UpdateProduct;
using Wave.Commerce.Domain.Entities.ProductEntity;
using Wave.Commerce.Domain.Entities.ProductEntity.Repositories;

namespace Wave.Commerce.Tests.Tests.ProductTests.Commands;

public class UpdateProductHandlerTest
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<ILogger<UpdateProductHandler>> _mockLogger;
    private readonly UpdateProductHandler _handler;
    private readonly Faker _faker;

    public UpdateProductHandlerTest()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockLogger = new Mock<ILogger<UpdateProductHandler>>();
        _handler = new UpdateProductHandler(_mockProductRepository.Object, _mockLogger.Object);
        _faker = new Faker();
    }

    [Fact]
    [Category("Unit")]
    public async Task Handle_ProductExists_ShouldUpdateProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();

        var product = Product.CreateEntity(
            _faker.Commerce.ProductName(),
            _faker.Random.Decimal(1, 1000),
            _faker.Random.Int(1, 100));

        var command = new UpdateProductCommand
        (
            productId,
            _faker.Commerce.ProductName(),
            _faker.Random.Decimal(),
            _faker.Random.Int(1, 100)
        );

        _mockProductRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).ReturnsAsync(product);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be($"Update Product fields with sucess, id: {command.ProductId}");

        _mockProductRepository.Verify(repo => repo.Update(It.IsAny<Product>()), Times.Once);
        _mockProductRepository.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Handle_RepositoryThrowsException_ShouldReturnError()
    {
        // Arrange
        var productId = Guid.NewGuid();

        var product = Product.CreateEntity(
            _faker.Commerce.ProductName(),
            _faker.Random.Decimal(1, 1000),
            _faker.Random.Int(1, 100));

        var command = new UpdateProductCommand
        (
            productId,
            _faker.Commerce.ProductName(),
            _faker.Random.Decimal(),
            _faker.Random.Int(1, 100)
        );

        var exceptionMessage = "Database failure";

        _mockProductRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).ReturnsAsync(product);
        _mockProductRepository.Setup(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();

        result.Error.Message.Should().Be($"Error when update Product fields: {exceptionMessage}");

        _mockProductRepository.Verify(repo => repo.Update(It.IsAny<Product>()), Times.Once);
        _mockProductRepository.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
