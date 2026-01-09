using EnterpriseInventoryApi.Domain.Entities;

namespace EnterpriseInventoryApi.Infrastructure.Repositories;

public interface IProductRepository
{
    IQueryable<Product> Query();
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Product product, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    void Remove(Product product);
}
