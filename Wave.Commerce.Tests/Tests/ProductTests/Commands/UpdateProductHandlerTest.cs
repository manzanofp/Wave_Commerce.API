using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Wave.Commerce.Application.Features.ProductFeatures.Commands.UpdateProduct;
using Wave.Commerce.Domain.Entities.ProductEntity;
using Wave.Commerce.Domain.Entities.ProductEntity.Repositories;
using Wave.Commerce.Tests.Shared;

namespace Wave.Commerce.Tests.Tests.ProductTests.Commands;

public class UpdateProductHandlerTest
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<ILogger<UpdateProductHandler>> _mockLogger;
    private readonly UpdateProductHandler _handler;
    private readonly FakeRequests _commands;
    private readonly FakeProduct _product;

    public UpdateProductHandlerTest()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockLogger = new Mock<ILogger<UpdateProductHandler>>();
        _handler = new UpdateProductHandler(_mockProductRepository.Object, _mockLogger.Object);
        _commands = new FakeRequests();
        _product = new FakeProduct();
    }

    [Fact]
    public async Task Handle_ProductExists_ShouldUpdateProduct()
    {
        // Arrange
        var product = _product.CreateValidEntity();
        var command = _commands.CreateValidUpdateCommand(product.Id, product.Name, product.Value, product.StockQuantity);

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
    public async Task Handle_ProductDoesNotExist_ShouldReturnError()
    {
        // Arrange
        var product = _product.CreateValidEntity();
        var command = _commands.CreateValidUpdateCommand(product.Id, product.Name,product.Value, product.StockQuantity);

        _mockProductRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).ReturnsAsync((Product)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be($"Product fields with id: {command.ProductId} not found");

        _mockProductRepository.Verify(repo => repo.Remove(It.IsAny<Product>()), Times.Never);
        _mockProductRepository.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_RepositoryThrowsException_ShouldReturnError()
    {
        // Arrange
        var product = _product.CreateValidEntity();
        var command = _commands.CreateValidUpdateCommand(product.Id, product.Name, product.Value, product.StockQuantity);

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
