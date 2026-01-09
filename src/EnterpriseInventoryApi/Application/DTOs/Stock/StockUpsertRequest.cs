using System.ComponentModel.DataAnnotations;

namespace EnterpriseInventoryApi.Application.DTOs.Stock;

public class StockUpsertRequest
{
    [Required]
    public Guid StoreId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}
