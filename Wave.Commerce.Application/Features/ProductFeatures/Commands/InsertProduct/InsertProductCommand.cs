using Wave.Commerce.Application.Abstractions;

namespace Wave.Commerce.Application.Features.ProductFeatures.Commands.InsertProduct;
public record InsertProductCommand(
    string Name,
    decimal Value,
    int StockQuantity
    ) : ICommand<Guid>;