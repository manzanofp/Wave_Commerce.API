using Microsoft.Extensions.Logging;
using Wave.Commerce.Application.Abstractions;
using Wave.Commerce.Application.Features.ProductFeatures.Commands.InsertProduct;
using Wave.Commerce.Domain.Entities.ProductEntity;
using Wave.Commerce.Domain.Entities.ProductEntity.Repositories;
using Wave.Commerce.Domain.Shared;

namespace Wave.Commerce.Application.Features.ProductFeatures.Commands.UpdateProduct;

public class UpdateProductHandler : ICommandHandler<UpdateProductCommand, string>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<UpdateProductHandler> _logger;

    public UpdateProductHandler(IProductRepository productRepository, ILogger<UpdateProductHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<Result<string>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Product? product = await _productRepository.GetById(request.ProductId);
            if (product == null)
            {
                _logger.LogWarning($"Product fields with id: {request.ProductId} not found");
                return Result.WithError<string>($"Product fields with id: {request.ProductId} not found");
            }

            product.UpdateFields(request.Name, request.Value, request.StockQuantity);

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync(cancellationToken);

            return Result.Success($"Update Product fields with sucess, id: {request.ProductId}");
        } 
        catch (Exception ex)
        {
            _logger.LogError($"Error when update Product fields with id: {request.ProductId}: {ex.Message}");
            return Result.WithError<string>($"Error when update Product fields: {ex.Message}");
        }
    }
}