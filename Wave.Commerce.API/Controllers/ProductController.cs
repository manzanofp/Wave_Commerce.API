using MediatR;
using Microsoft.AspNetCore.Mvc;
using Wave.Commerce.API.Controllers.Base;
using Wave.Commerce.Application.Features.ProductFeatures.Commands.InsertProduct;
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

}
