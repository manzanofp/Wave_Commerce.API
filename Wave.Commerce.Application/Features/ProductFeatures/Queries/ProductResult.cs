namespace Wave.Commerce.Application.Features.ProductFeatures.Queries;

public record ProductResult(
    Guid? Id,
    string? Name,
    decimal? Value,
    int? StockQuantity);
