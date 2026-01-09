namespace EnterpriseInventoryApi.Domain.Entities;

public class Stock
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid StoreId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Store? Store { get; set; }
    public Product? Product { get; set; }
}
