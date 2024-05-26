using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Wave.Commerce.Application.Features.ProductFeatures.Commands.DeleteProduct;
using Wave.Commerce.Domain.Entities.ProductEntity;
using Wave.Commerce.Domain.Entities.ProductEntity.Repositories;
using Wave.Commerce.Tests.Shared;

namespace Wave.Commerce.Tests.Tests.ProductTests.Commands;

public class DeleteProductHandlerTest
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<ILogger<DeleteProductHandler>> _mockLogger;
    private readonly DeleteProductHandler _handler;
    private readonly FakeRequests _commands;
    private readonly FakeProduct _product;

    public DeleteProductHandlerTest()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockLogger = new Mock<ILogger<DeleteProductHandler>>();
        _handler = new DeleteProductHandler(_mockProductRepository.Object, _mockLogger.Object);
        _commands = new FakeRequests();
        _product = new FakeProduct();
    }

    [Fact]
    public async Task Handle_ProductExists_ShouldDeleteProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = _commands.CreateValidDeleteCommand(productId);
        var product = _product.CreateValidEntity();

        _mockProductRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).ReturnsAsync(product);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Product deleted with success");

        _mockProductRepository.Verify(repo => repo.Remove(product), Times.Once);
        _mockProductRepository.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }


    [Fact]
    public async Task Handle_ProductDoesNotExist_ShouldReturnError()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = _commands.CreateValidDeleteCommand(productId);

        _mockProductRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).ReturnsAsync((Product)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be($"Product with id:  {command.ProductId} not found");

        _mockProductRepository.Verify(repo => repo.Remove(It.IsAny<Product>()), Times.Never);
        _mockProductRepository.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_RepositoryThrowsException_ShouldReturnError()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = _commands.CreateValidDeleteCommand(productId);
        var product = _product.CreateValidEntity();

        var exceptionMessage = "Database failure";

        _mockProductRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).ReturnsAsync(product);
        _mockProductRepository.Setup(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be($"Error when try delete Product with id: {exceptionMessage}");

        _mockProductRepository.Verify(repo => repo.Remove(product), Times.Once);
        _mockProductRepository.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
