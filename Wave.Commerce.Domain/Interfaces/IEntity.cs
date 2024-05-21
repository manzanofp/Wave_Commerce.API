namespace Wave.Commerce.Domain.Interfaces;

public interface IEntity
{
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset UpdatedDate { get; set; }
}

public interface IEntity<TKey> : IEntity
{
    TKey Id { get; }
}
