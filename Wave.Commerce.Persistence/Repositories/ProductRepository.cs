using Microsoft.EntityFrameworkCore;
using Wave.Commerce.Domain.Entities.ProductEntity;
using Wave.Commerce.Domain.Entities.ProductEntity.Repositories;
using Wave.Commerce.Persistence.Context;
using Wave.Commerce.Persistence.Repositories.Base;

namespace Wave.Commerce.Persistence.Repositories;
public class ProductRepository : BaseRepository<Product, Guid>, IProductRepository
{
    public ProductRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public Task<Product?> GetByName(string name) => Get().Where(x => x.Name == name).FirstOrDefaultAsync();
}
