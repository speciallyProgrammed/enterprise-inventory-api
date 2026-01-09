namespace EnterpriseInventoryApi.Application.DTOs.Stock;

public class StockDto
{
    public Guid StoreId { get; set; }
    public Guid ProductId { get; set; }
    public string StoreName { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime UpdatedAt { get; set; }
}
