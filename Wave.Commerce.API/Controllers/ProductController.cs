using MediatR;
using Microsoft.AspNetCore.Mvc;
using Wave.Commerce.API.Controllers.Base;
using Wave.Commerce.Application.Features.ProductFeatures.Commands.DeleteProduct;
using Wave.Commerce.Application.Features.ProductFeatures.Commands.InsertProduct;
using Wave.Commerce.Application.Features.ProductFeatures.Commands.UpdateProduct;
using Wave.Commerce.Application.Features.ProductFeatures.Queries;
using Wave.Commerce.Application.Features.ProductFeatures.Queries.ListProductByName;
using Wave.Commerce.Application.Features.ProductFeatures.Queries.ListProductOrderedBy;
using Wave.Commerce.Domain.Shared;

namespace Wave.Commerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : WaveCommerceBaseController
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    public async Task<IActionResult> Insert(InsertProductCommand command, CancellationToken cancellationToken)
    {
        Result<Guid> result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : HandleFailure(result);
    }

    [HttpPut]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    public async Task<IActionResult> Update(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        Result<string> result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : HandleFailure(result);
    }

    [HttpDelete("delete/{productId:Guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> Delete(Guid productId, CancellationToken cancellationToken)
    {
        Result<string> result = await _mediator.Send(new DeleteProductCommand(productId), cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : HandleFailure(result);
    }

    [HttpGet("list/{productName}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> GetByName(string productName, CancellationToken cancellationToken)
    {
        Result<List<ProductResult>> result = await _mediator.Send(new ListProductByNameQuery(productName), cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : HandleFailure(result);
    }

    [HttpGet("orderBy/{field}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> GetByFields(string field, int? stockValue, CancellationToken cancellationToken)
    {
        Result<List<ProductResult>> result = await _mediator.Send(new ListProductOrderByFieldsQuery(field, stockValue), cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : HandleFailure(result);
    }
}
