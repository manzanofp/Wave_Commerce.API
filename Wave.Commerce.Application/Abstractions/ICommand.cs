using MediatR;
using Wave.Commerce.Domain.Shared;

namespace Wave.Commerce.Application.Abstractions;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
