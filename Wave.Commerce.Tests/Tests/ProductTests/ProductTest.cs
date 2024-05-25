using Bogus;
using FluentAssertions;
using System.ComponentModel;
using Wave.Commerce.Domain.Entities.ProductEntity;

namespace Wave.Commerce.Tests.Tests.ProductTests;

public class ProductTest
{
    private readonly Faker _faker;

    public ProductTest() 
    {
        _faker = new Faker();
    }

    [Fact]
    public void CreateEntity_ShouldThrowArgumentOutOfRangeException_WhenValueIsNegative()
    {
        // Arrange
        var name = _faker.Commerce.ProductName();
        var value = _faker.Random.Decimal(-1000, -1);
        var stockQuantity = _faker.Random.Int(1, 100);

        // Act
        Action act = () => Product.CreateEntity(name, value, stockQuantity);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("Value cannot be negative. (Parameter 'value')");
    }

    [Fact]
    public void CreateEntity_ShouldThrowArgumentOutOfRangeException_WhenStockQuantityIsNegative()
    {
        // Arrange
        var name = _faker.Commerce.ProductName();
        var value = _faker.Random.Decimal(1, 1000);
        var stockQuantity = _faker.Random.Int(-1000, -1);

        // Act
        Action act = () => Product.CreateEntity(name, value, stockQuantity);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("StockQuantity cannot be negative. (Parameter 'stockQuantity')");
    }
}
