using Wave.Commerce.Application.Abstractions;

namespace Wave.Commerce.Application.Features.ProductFeatures.Queries.ListProductOrderedBy;

public record ListProductOrderByFieldsQuery(string Field, int? StockValue) : IQuery<List<ProductResult>>;
