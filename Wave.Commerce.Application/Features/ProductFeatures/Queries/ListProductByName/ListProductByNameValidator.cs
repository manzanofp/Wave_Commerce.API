using FluentValidation;

namespace Wave.Commerce.Application.Features.ProductFeatures.Queries.ListProductByName;

public class ListProductByNameValidator : AbstractValidator<ListProductByNameQuery>
{
    public ListProductByNameValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .WithMessage("The name field cannot be empty or null");
    }
}
