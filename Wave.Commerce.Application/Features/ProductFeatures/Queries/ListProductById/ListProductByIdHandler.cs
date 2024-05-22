using Microsoft.Extensions.Logging;
using Wave.Commerce.Application.Abstractions;
using Wave.Commerce.Domain.Entities.ProductEntity.Repositories;
using Wave.Commerce.Domain.Shared;

namespace Wave.Commerce.Application.Features.ProductFeatures.Queries.ListProductById;

public class ListProductByIdHandler : IQueryHandler<ListProductByIdQuery, ProductResult>
{

    private readonly IProductRepository _productRepository;
    private readonly ILogger<ListProductByIdHandler> _logger;

    public ListProductByIdHandler(IProductRepository productRepository, ILogger<ListProductByIdHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<Result<ProductResult>> Handle(ListProductByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var productById = await _productRepository.GetById(request.ProductId);
            if (productById == null)
            {
                _logger.LogWarning($"Product with Id: {request.ProductId} not found.");
                return Result.WithError<ProductResult>($"Product with Id:  {request.ProductId} not found.");
            }

            var result = new ProductResult(
                productById.Id,
                productById.Name,
                productById.Value,
                productById.StockQuantity
                );

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error When trying to search product By Id: {request.ProductId}, {ex.Message}");
            return Result.WithError<ProductResult>($"Error When trying to search product By Id: {ex.Message}");
        }
    }
}
