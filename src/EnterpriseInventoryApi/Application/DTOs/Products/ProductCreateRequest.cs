using System.ComponentModel.DataAnnotations;

namespace EnterpriseInventoryApi.Application.DTOs.Products;

public class ProductCreateRequest
{
    [Required]
    [StringLength(100)]
    public string Sku { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, 1000000)]
    public decimal Price { get; set; }
}
