using System.ComponentModel.DataAnnotations;

namespace EnterpriseInventoryApi.Application.DTOs.Stores;

public class StoreUpdateRequest
{
    [Required]
    [StringLength(100)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
}
