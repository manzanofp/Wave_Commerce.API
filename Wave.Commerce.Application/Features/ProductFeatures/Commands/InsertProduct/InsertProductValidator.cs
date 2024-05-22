using FluentValidation;

namespace Wave.Commerce.Application.Features.ProductFeatures.Commands.InsertProduct;

public class InsertProductValidator : AbstractValidator<InsertProductCommand>
{
    public InsertProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .WithMessage("The name field cannot be empty or null");

        RuleFor(x => x.Value)
            .NotEmpty()
            .NotNull()
            .GreaterThanOrEqualTo(0m)
            .WithMessage("The value field cannot be negative");

        RuleFor(x => x.StockQuantity)
            .NotEmpty()
            .NotNull()
            .GreaterThanOrEqualTo(0)
            .WithMessage("The StockQuantity field cannot be negative");
    }
}
