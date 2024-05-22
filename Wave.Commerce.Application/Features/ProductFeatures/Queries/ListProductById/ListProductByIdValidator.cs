using FluentValidation;

namespace Wave.Commerce.Application.Features.ProductFeatures.Queries.ListProductById;
public class ListProductByIdValidator : AbstractValidator<ListProductByIdQuery>
{
    public ListProductByIdValidator()
    {
        RuleFor(x => x.ProductId)
            .NotNull()
            .NotEmpty()
            .WithMessage("The ProductId field cannot be empty or null");
    }
}
