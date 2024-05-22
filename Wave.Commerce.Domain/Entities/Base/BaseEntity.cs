using Wave.Commerce.Domain.Interfaces;

namespace Wave.Commerce.Domain.Entities.Base;

public abstract class BaseEntity : IEntity<Guid>
{

    public BaseEntity()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset UpdatedDate { get; set; }
}
