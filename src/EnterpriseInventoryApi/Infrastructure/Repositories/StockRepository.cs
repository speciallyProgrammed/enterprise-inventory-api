using EnterpriseInventoryApi.Domain.Entities;
using EnterpriseInventoryApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseInventoryApi.Infrastructure.Repositories;

public class StockRepository : IStockRepository
{
    private readonly AppDbContext _db;

    public StockRepository(AppDbContext db)
    {
        _db = db;
    }

    public IQueryable<Stock> Query()
    {
        return _db.Stocks.Include(s => s.Product).Include(s => s.Store).AsQueryable();
    }

    public Task<Stock?> GetByStoreAndProductAsync(Guid storeId, Guid productId, CancellationToken cancellationToken)
    {
        return _db.Stocks.FirstOrDefaultAsync(s => s.StoreId == storeId && s.ProductId == productId, cancellationToken);
    }

    public async Task AddAsync(Stock stock, CancellationToken cancellationToken)
    {
        await _db.Stocks.AddAsync(stock, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _db.SaveChangesAsync(cancellationToken);
    }
}
