using EnterpriseInventoryApi.Application.DTOs.Stock;
using EnterpriseInventoryApi.Common.Pagination;

namespace EnterpriseInventoryApi.Application.Services;

public interface IStockService
{
    Task<PagedResult<StockDto>> GetAsync(Guid? storeId, Guid? productId, int page, int pageSize, CancellationToken cancellationToken);
    Task<StockDto> UpsertAsync(StockUpsertRequest request, CancellationToken cancellationToken);
}
