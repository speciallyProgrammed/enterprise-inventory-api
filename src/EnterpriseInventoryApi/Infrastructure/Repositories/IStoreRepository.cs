using EnterpriseInventoryApi.Domain.Entities;

namespace EnterpriseInventoryApi.Infrastructure.Repositories;

public interface IStoreRepository
{
    IQueryable<Store> Query();
    Task<Store?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Store store, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    void Remove(Store store);
}
