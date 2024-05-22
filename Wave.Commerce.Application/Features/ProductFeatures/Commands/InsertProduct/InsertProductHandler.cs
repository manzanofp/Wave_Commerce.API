using Microsoft.Extensions.Logging;
using Wave.Commerce.Application.Abstractions;
using Wave.Commerce.Domain.Entities.ProductEntity;
using Wave.Commerce.Domain.Entities.ProductEntity.Repositories;
using Wave.Commerce.Domain.Shared;

namespace Wave.Commerce.Application.Features.ProductFeatures.Commands.InsertProduct;

public class InsertProductHandler : ICommandHandler<InsertProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<InsertProductHandler> _logger;

    public InsertProductHandler(IProductRepository productRepository, ILogger<InsertProductHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(InsertProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = Product.CreateEntity(request.Name, request.Value, request.StockQuantity);

            _productRepository.Add(product);
            await _productRepository.SaveChangesAsync(cancellationToken);

            return Result.Success(product.Id);

        } catch (Exception ex)
        {
            _logger.LogError($"Error when trying to insert product:{request} ,{ex.Message}");
            return Result.WithError<Guid>($"Error when trying to insert product:{request}, {ex.Message}");
        }
    }
}
