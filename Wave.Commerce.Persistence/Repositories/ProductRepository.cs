using Microsoft.EntityFrameworkCore;
using Wave.Commerce.Domain.Entities.ProductEntity;
using Wave.Commerce.Domain.Entities.ProductEntity.Repositories;
using Wave.Commerce.Persistence.Context;
using Wave.Commerce.Persistence.Repositories.Base;

namespace Wave.Commerce.Persistence.Repositories;
public class ProductRepository : BaseRepository<Product, Guid>, IProductRepository
{
    public ProductRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public Task<Product?> GetById(Guid id) => Get().Where(x => x.Id == id).FirstOrDefaultAsync();

    public Task<List<Product>> GetByName(string name) => Get().Where(x => x.Name.Contains(name)).ToListAsync();

    public async Task<List<Product>> GetOrderByFields(string field, int? stockValue)
    {
        IQueryable<Product> query = Get();

        switch (field.ToLower())
        {
            case "name":
                query = query.OrderBy(x => x.Name);
                break;
            case "stock":
                if (stockValue.HasValue)
                    query = query.Where(x => x.StockQuantity == stockValue.Value);
                else
                    query = query.OrderByDescending(x => x.StockQuantity);
                break;
            case "value":
                query = query.OrderByDescending(x => x.Value);
                break;
            default:
                query = query.OrderBy(x => x.Id);
                break;
        }

        return await query.ToListAsync();
    }
}
