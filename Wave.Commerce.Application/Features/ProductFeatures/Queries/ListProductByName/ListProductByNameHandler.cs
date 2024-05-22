using Microsoft.Extensions.Logging;
using Wave.Commerce.Application.Abstractions;
using Wave.Commerce.Domain.Entities.ProductEntity.Repositories;
using Wave.Commerce.Domain.Shared;

namespace Wave.Commerce.Application.Features.ProductFeatures.Queries.ListProductByName;
public class ListProductByNameHandler : IQueryHandler<ListProductByNameQuery, List<ProductResult>>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ListProductByNameHandler> _logger;

    public ListProductByNameHandler(IProductRepository productRepository, ILogger<ListProductByNameHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<Result<List<ProductResult>>> Handle(ListProductByNameQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var productByName = await _productRepository.GetByName(request.Name);
            if (productByName == null)
            {
                _logger.LogWarning($"Product with name: {request.Name} not found.");
                return Result.WithError<List<ProductResult>>($"Product with name:  {request.Name} not found.");
            }

            var result = productByName.Select(x => new ProductResult(
                x.Id,
                x.Name,
                x.Value,
                x.StockQuantity)).ToList();

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error When trying to List products By Name: {request.Name}, {ex.Message}");
            return Result.WithError<List<ProductResult>>($"Error When trying to List products By Name: {ex.Message}");
        }
    }

}
