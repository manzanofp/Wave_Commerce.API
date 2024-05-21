using Wave.Commerce.Domain.Shared;

namespace Wave.Commerce.Domain.Interfaces;

public interface IValidationResult
{
    public Error[] Errors { get; }
}
