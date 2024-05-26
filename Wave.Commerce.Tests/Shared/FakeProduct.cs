using Bogus;
using Wave.Commerce.Application.Features.ProductFeatures.Queries;
using Wave.Commerce.Domain.Entities.ProductEntity;

namespace Wave.Commerce.Tests.Shared;

public class FakeProduct
{
    private readonly Faker _faker = new();

    public Product CreateValidEntity()
    {
        var product = Product.CreateEntity(_faker.Commerce.ProductName(),
            _faker.Random.Decimal(0m, 9999m),
            _faker.Random.Int(0, 9999));

        return product;
    }

    public List<Product> CreateProductList(string productName)
    {
        return new List<Product>
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
            ),
            Product.CreateEntity(
                productName,
                _faker.Random.Decimal(1, 1000),
                _faker.Random.Int(1, 100)
            )
        };
    }

    public ProductResult MapToProductResult(Product product)
    {
        return new ProductResult(
            product.Id,
            product.Name,
            product.Value,
            product.StockQuantity
        );
    }
}
