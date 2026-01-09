using EnterpriseInventoryApi.Application.DTOs.Products;
using EnterpriseInventoryApi.Common.Pagination;

namespace EnterpriseInventoryApi.Application.Services;

public interface IProductService
{
    Task<PagedResult<ProductDto>> GetAsync(int page, int pageSize, string? search, CancellationToken cancellationToken);
    Task<ProductDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<ProductDto> CreateAsync(ProductCreateRequest request, CancellationToken cancellationToken);
    Task<ProductDto> UpdateAsync(Guid id, ProductUpdateRequest request, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
