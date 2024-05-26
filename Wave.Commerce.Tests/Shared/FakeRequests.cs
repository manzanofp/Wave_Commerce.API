using Bogus;
using Wave.Commerce.Application.Features.ProductFeatures.Commands.DeleteProduct;
using Wave.Commerce.Application.Features.ProductFeatures.Commands.InsertProduct;
using Wave.Commerce.Application.Features.ProductFeatures.Commands.UpdateProduct;
using Wave.Commerce.Application.Features.ProductFeatures.Queries.ListProductById;
using Wave.Commerce.Application.Features.ProductFeatures.Queries.ListProductByName;
using Wave.Commerce.Application.Features.ProductFeatures.Queries.ListProductOrderedBy;

namespace Wave.Commerce.Tests.Shared;
public class FakeRequests
{
    private readonly Faker _Faker = new();

    public InsertProductCommand CreateValidInsertCommand()
    {
        return new InsertProductCommand(_Faker.Commerce.ProductName(), _Faker.Random.Decimal(0m,9999m), _Faker.Random.Int(0, 9999));
    }

    public DeleteProductCommand CreateValidDeleteCommand(Guid productId)
    {
        return new DeleteProductCommand(productId);
    }

    public UpdateProductCommand CreateValidUpdateCommand(Guid productId, string name, decimal value, int stockQuantity)
    {
        return new UpdateProductCommand(productId, name, value, stockQuantity);
    }

    public ListProductByIdQuery CreateValidListProductByIdQuery(Guid productId)
    {
        return new ListProductByIdQuery(productId);
    }

    public ListProductByNameQuery CreateValidListProductByNameQuery(string name)
    {
        return new ListProductByNameQuery(name);
    }

    public ListProductOrderByFieldsQuery CreateValidListProductOrderByQuery(string field, int? stockQuantity)
    {
        return new ListProductOrderByFieldsQuery(field, stockQuantity);
    }
}
