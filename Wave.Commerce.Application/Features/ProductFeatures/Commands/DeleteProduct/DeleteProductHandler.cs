using Microsoft.Extensions.Logging;
using Wave.Commerce.Application.Abstractions;
using Wave.Commerce.Domain.Entities.ProductEntity;
using Wave.Commerce.Domain.Entities.ProductEntity.Repositories;
using Wave.Commerce.Domain.Shared;

namespace Wave.Commerce.Application.Features.ProductFeatures.Commands.DeleteProduct;
public class DeleteProductHandler : ICommandHandler<DeleteProductCommand, string>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<DeleteProductHandler> _logger;

    public DeleteProductHandler(IProductRepository productRepository, ILogger<DeleteProductHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<Result<string>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Product? product = await _productRepository.GetById(request.ProductId);
            if(product == null)
            {
                _logger.LogWarning($"Product with id: {request.ProductId} not found");
                return Result.WithError<string>($"Product with id:  {request.ProductId} not found");
            }

            _productRepository.Remove(product);
            await _productRepository.SaveChangesAsync(cancellationToken);

            return Result.Success("Product deleted with success");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error when try delete Product with id: {request.ProductId}: {ex.Message}");
            return Result.WithError<string>($"Error when try delete Product with id: {ex.Message}");
        }
    }
}
