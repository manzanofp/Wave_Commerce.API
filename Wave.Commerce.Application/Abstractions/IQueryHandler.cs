using MediatR;
using Wave.Commerce.Domain.Shared;

namespace Wave.Commerce.Application.Abstractions;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
