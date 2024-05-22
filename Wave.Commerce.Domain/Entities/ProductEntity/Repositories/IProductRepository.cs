using Wave.Commerce.Domain.Interfaces;

namespace Wave.Commerce.Domain.Entities.ProductEntity.Repositories;

public interface IProductRepository : IBaseRepository<Product, Guid>
{
    Task<Product?> GetById(Guid id);
    Task<List<Product>> GetByName(string name);
    Task<List<Product>> GetOrderByFields(string field, int? stockValue);
}
