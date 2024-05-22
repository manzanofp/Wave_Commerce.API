using Wave.Commerce.Application.Abstractions;
using Wave.Commerce.Domain.Shared;

namespace Wave.Commerce.Application.Features.ProductFeatures.Commands.InsertProduct;

public class InsertProductHandler : ICommandHandler<InsertProductCommand, Guid>
{


    public Task<Result<Guid>> Handle(InsertProductCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
