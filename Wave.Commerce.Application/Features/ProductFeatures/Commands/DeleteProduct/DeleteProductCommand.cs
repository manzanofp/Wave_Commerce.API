using Wave.Commerce.Application.Abstractions;

namespace Wave.Commerce.Application.Features.ProductFeatures.Commands.DeleteProduct;
public record DeleteProductCommand(Guid ProductId) : ICommand<string>;
