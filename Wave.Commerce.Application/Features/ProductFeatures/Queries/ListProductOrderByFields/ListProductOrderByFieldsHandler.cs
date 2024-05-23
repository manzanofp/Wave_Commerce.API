using Microsoft.Extensions.Logging;
using Wave.Commerce.Application.Abstractions;
using Wave.Commerce.Application.Features.ProductFeatures.Queries.ListProductOrderedBy;
using Wave.Commerce.Domain.Entities.ProductEntity.Repositories;
using Wave.Commerce.Domain.Shared;

namespace Wave.Commerce.Application.Features.ProductFeatures.Queries.ListProductOrderByFields;

public class ListProductOrderByFieldsHandler : IQueryHandler<ListProductOrderByFieldsQuery, List<ProductResult>>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ListProductOrderByFieldsHandler> _logger;

    public ListProductOrderByFieldsHandler(IProductRepository productRepository, ILogger<ListProductOrderByFieldsHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<Result<List<ProductResult>>> Handle(ListProductOrderByFieldsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var productOrderBy = await _productRepository.GetOrderByFields(request.Field, request.StockValue);
            if (productOrderBy == null)
            {
                _logger.LogWarning($"Product order by field: {request.Field} not found.");
                return Result.WithError<List<ProductResult>>($"Product order by field: {request.Field} not found.");
            }

            var result = productOrderBy.Select(x => new ProductResult(
                x.Id,
                x.Name,
                x.Value,
                x.StockQuantity)).ToList();

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error When trying to List products Order By fields: {request.Field}, {ex.Message}");
            return Result.WithError<List<ProductResult>>($"Error When trying to List products Order By fields: {ex.Message}");
        }
    }
}
