using Wave.Commerce.Application.Features.ProductFeatures.Queries;
using Wave.Commerce.Domain.Entities.ProductEntity;

namespace Wave.Commerce.IntegrationTests.Shared;

public class FakeProduct
{
    public static ProductResult FakeProductResult()
    {
        return new ProductResult(Guid.Parse("8270d170-d487-4a1b-8f0c-12eee8ca4a4a"), "Dell XPS 13 Laptop", 999.80m, 150);
    }

    public static List<Product> FakeListProduct()
    {
        var products = new List<Product>
        {
            Product.CreateEntity(Guid.Parse("076d553e-4bf8-4367-931a-e7742501e8b6"), "Apple Iphone 13", 799.99m, 50),
            Product.CreateEntity(Guid.Parse("3c253a59-f32c-4ff1-9ab7-af245e50e291"), "Samsung Galaxy S21", 699.99m, 70),
            Product.CreateEntity(Guid.Parse("8270d170-d487-4a1b-8f0c-12eee8ca4a4a"), "Dell XPS 13 Laptop", 999.80m, 150),
            Product.CreateEntity(Guid.Parse("02664afa-6ab9-4ef7-9619-b3644f9c02e4"), "Amazon Echo Dot (4th Gen)", 60.50m, 350),
            Product.CreateEntity(Guid.Parse("7bec3fbb-94cb-4d09-976b-d0831b3b9ad8"), "Galaxy Watch pro 5", 1260.50m, 350),
        };

        return products;
    }
}
