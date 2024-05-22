using Wave.Commerce.Domain.Interfaces;

namespace Wave.Commerce.Domain.Entities.ProductEntity.Repositories;

public interface IProductRepository : IBaseRepository<Product, Guid>
{
    Task<Product?> GetById(Guid id);
    Task<Product?> GetByName(string name);   
}
