using FluentValidation;

namespace Wave.Commerce.Application.Features.ProductFeatures.Commands.DeleteProduct;
public class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductValidator()
    {
        RuleFor(x => x.ProductId)
            .NotNull()
            .NotEmpty()
            .WithMessage("The ProductId field cannot be empty or null");
    }
}
