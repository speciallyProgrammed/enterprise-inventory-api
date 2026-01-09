using EnterpriseInventoryApi.Domain.Entities;
using EnterpriseInventoryApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseInventoryApi.Infrastructure.Repositories;

public class StoreRepository : IStoreRepository
{
    private readonly AppDbContext _db;

    public StoreRepository(AppDbContext db)
    {
        _db = db;
    }

    public IQueryable<Store> Query()
    {
        return _db.Stores.AsQueryable();
    }

    public Task<Store?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _db.Stores.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task AddAsync(Store store, CancellationToken cancellationToken)
    {
        await _db.Stores.AddAsync(store, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _db.SaveChangesAsync(cancellationToken);
    }

    public void Remove(Store store)
    {
        _db.Stores.Remove(store);
    }
}
