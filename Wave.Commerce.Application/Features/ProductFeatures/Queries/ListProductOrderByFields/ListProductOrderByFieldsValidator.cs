using FluentValidation;
using Wave.Commerce.Application.Features.ProductFeatures.Queries.ListProductOrderedBy;

namespace Wave.Commerce.Application.Features.ProductFeatures.Queries.ListProductOrderByFields;

public class ListProductOrderByFieldsValidator : AbstractValidator<ListProductOrderByFieldsQuery>
{
    public ListProductOrderByFieldsValidator()
    {
        RuleFor(x => x.Field)
            .NotEmpty()
            .NotNull()
            .WithMessage("The field for order by must be a valid propertie in Product");
    }
}
