using Wave.Commerce.Application.Abstractions;

namespace Wave.Commerce.Application.Features.ProductFeatures.Queries.ListProductByName;
public record ListProductByNameQuery(string Name) : IQuery<List<ProductResult>>;
