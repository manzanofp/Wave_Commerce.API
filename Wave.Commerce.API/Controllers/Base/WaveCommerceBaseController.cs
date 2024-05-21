using Microsoft.AspNetCore.Mvc;
using Wave.Commerce.Domain.Interfaces;
using Wave.Commerce.Domain.Shared;

namespace Wave.Commerce.API.Controllers.Base;

public class WaveCommerceBaseController : ControllerBase
{
    protected IActionResult HandleFailure(Result result)
    {
        return result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),

            IValidationResult validationResult =>
                BadRequest(CreateProblemDetails(
                    "Validation Error",
                    StatusCodes.Status400BadRequest,
                    result.Error,
                    validationResult.Errors)),
            _ =>
                BadRequest(CreateProblemDetails(
                    "Bad Request",
                    StatusCodes.Status400BadRequest,
                    result.Error))
        };
    }

    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error error,
        Error[]? errors = null)
    {
        var problemDetails = new ProblemDetails()
        {
            Title = title,
            Detail = error.Message,
            Status = status,
        };

        if (errors is not null)
        {
            problemDetails.Extensions.Add(nameof(errors), errors);
        }

        return problemDetails;
    }
}
