using Wave.Commerce.Domain.Entities.Base;

namespace Wave.Commerce.Domain.Entities.ProductEntity;
public class Product : BaseEntity
{
    private Product(
        string name, 
        decimal value,
        int stockQuantity
        )
    {
        Name = name;
        Value = value;
        StockQuantity = stockQuantity;
    }


    #region Properties

    public string Name { get; set; }
    public decimal Value { get; set; }
    public int StockQuantity { get; set; }

    #endregion

    public static Product CreateEntity(string name, decimal value, int stockQuantity)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty", nameof(name));

        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Value cannot be negative.");

        if (stockQuantity < 0)
            throw new ArgumentOutOfRangeException(nameof(stockQuantity), "StockQuantity cannot be negative.");

        return new Product(name, value, stockQuantity);
    }
}
