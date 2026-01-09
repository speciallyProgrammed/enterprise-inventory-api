using EnterpriseInventoryApi.Domain.Entities;

namespace EnterpriseInventoryApi.Infrastructure.Repositories;

public interface IStockRepository
{
    IQueryable<Stock> Query();
    Task<Stock?> GetByStoreAndProductAsync(Guid storeId, Guid productId, CancellationToken cancellationToken);
    Task AddAsync(Stock stock, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
