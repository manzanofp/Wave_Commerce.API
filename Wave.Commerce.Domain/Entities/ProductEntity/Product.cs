namespace Wave.Commerce.Domain.Entities.ProductEntity;
public class Product
{
    public Product(
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
}
