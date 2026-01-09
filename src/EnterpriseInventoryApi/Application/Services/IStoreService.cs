using EnterpriseInventoryApi.Application.DTOs.Stores;
using EnterpriseInventoryApi.Common.Pagination;

namespace EnterpriseInventoryApi.Application.Services;

public interface IStoreService
{
    Task<PagedResult<StoreDto>> GetAsync(int page, int pageSize, string? search, CancellationToken cancellationToken);
    Task<StoreDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<StoreDto> CreateAsync(StoreCreateRequest request, CancellationToken cancellationToken);
    Task<StoreDto> UpdateAsync(Guid id, StoreUpdateRequest request, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
