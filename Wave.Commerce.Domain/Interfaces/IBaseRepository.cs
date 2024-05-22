namespace Wave.Commerce.Domain.Interfaces;

public interface IBaseRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    void Add(TEntity entity);
    void Update(TEntity entity);
    IQueryable<TEntity> Get();
    IQueryable<TEntity> GetAsNoTracking();
    void Remove(TEntity entity);
    Task SaveChangesAsync();
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
