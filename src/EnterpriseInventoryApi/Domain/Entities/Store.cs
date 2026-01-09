namespace EnterpriseInventoryApi.Domain.Entities;

public class Store
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
