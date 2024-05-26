using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Wave.Commerce.Application.Features.ProductFeatures.Commands.InsertProduct;
using Wave.Commerce.Domain.Entities.ProductEntity;
using Wave.Commerce.Domain.Entities.ProductEntity.Repositories;
using Wave.Commerce.Tests.Shared;

namespace Wave.Commerce.Tests.Tests.ProductTests.Commands;

public class InsertProductHandlerTest
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<ILogger<InsertProductHandler>> _mockLogger;
    private readonly InsertProductHandler _handler;
    private readonly FakeRequests _commands;
    private readonly FakeProduct _product;

    public InsertProductHandlerTest()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockLogger = new Mock<ILogger<InsertProductHandler>>();
        _handler = new InsertProductHandler(_mockProductRepository.Object, _mockLogger.Object);
        _commands = new FakeRequests();
        _product = new FakeProduct();
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenProductIsInsertedSuccessfully()
    {
        // Arrange
        var command = _commands.CreateValidInsertCommand();
        var product = _product.CreateValidEntity();

        product.Id = Guid.NewGuid();

        _mockProductRepository
            .Setup(repo => repo.Add(It.IsAny<Product>()))
            .Callback<Product>(p => p.Id = product.Id);

        _mockProductRepository
            .Setup(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(product.Id);

        _mockProductRepository.Verify(repo => repo.Add(It.IsAny<Product>()), Times.Once);
        _mockProductRepository.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnErrorResult_WhenExceptionIsThrown()
    {
        // Arrange
        var command = _commands.CreateValidInsertCommand();
        var product = _product.CreateValidEntity();

        var exceptionMessage = "Database failure";

        _mockProductRepository
            .Setup(repo => repo.Add(It.IsAny<Product>()))
            .Throws(new Exception(exceptionMessage));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Message.Should().Be($"Error when trying to insert product:{command}, {exceptionMessage}");
    }
}
