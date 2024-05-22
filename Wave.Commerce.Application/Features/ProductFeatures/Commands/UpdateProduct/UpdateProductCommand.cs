using Wave.Commerce.Application.Abstractions;

namespace Wave.Commerce.Application.Features.ProductFeatures.Commands.UpdateProduct;

public record UpdateProductCommand(
    Guid ProductId,
    string Name,
    decimal Value,
    int StockQuantity) : ICommand<string>;
