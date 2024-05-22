using MediatR;
using Wave.Commerce.Domain.Shared;

namespace Wave.Commerce.Application.Abstractions;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
