using Wave.Commerce.Application.Abstractions;

namespace Wave.Commerce.Application.Features.ProductFeatures.Queries.ListProductById;

public record ListProductByIdQuery(Guid ProductId): IQuery<ProductResult>;
