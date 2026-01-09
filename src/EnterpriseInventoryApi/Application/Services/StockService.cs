using EnterpriseInventoryApi.Application.DTOs.Stock;
using EnterpriseInventoryApi.Common.Errors;
using EnterpriseInventoryApi.Common.Pagination;
using EnterpriseInventoryApi.Domain.Entities;
using EnterpriseInventoryApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseInventoryApi.Application.Services;

public class StockService : IStockService
{
    private readonly IStockRepository _stocks;
    private readonly IStoreRepository _stores;
    private readonly IProductRepository _products;

    public StockService(IStockRepository stocks, IStoreRepository stores, IProductRepository products)
    {
        _stocks = stocks;
        _stores = stores;
        _products = products;
    }

    public async Task<PagedResult<StockDto>> GetAsync(Guid? storeId, Guid? productId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _stocks.Query();

        if (storeId.HasValue)
        {
            query = query.Where(s => s.StoreId == storeId.Value);
        }

        if (productId.HasValue)
        {
            query = query.Where(s => s.ProductId == productId.Value);
        }

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(s => s.Store!.Name)
            .ThenBy(s => s.Product!.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new StockDto
            {
                StoreId = s.StoreId,
                ProductId = s.ProductId,
                StoreName = s.Store!.Name,
                ProductName = s.Product!.Name,
                Quantity = s.Quantity,
                UpdatedAt = s.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<StockDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = total
        };
    }

    public async Task<StockDto> UpsertAsync(StockUpsertRequest request, CancellationToken cancellationToken)
    {
        var storeExists = await _stores.Query().AnyAsync(s => s.Id == request.StoreId, cancellationToken);
        if (!storeExists)
        {
            throw new ApiException(ErrorCodes.NotFound, "Store not found", 404);
        }

        var productExists = await _products.Query().AnyAsync(p => p.Id == request.ProductId, cancellationToken);
        if (!productExists)
        {
            throw new ApiException(ErrorCodes.NotFound, "Product not found", 404);
        }

        var stock = await _stocks.GetByStoreAndProductAsync(request.StoreId, request.ProductId, cancellationToken);
        if (stock == null)
        {
            stock = new Stock
            {
                StoreId = request.StoreId,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                UpdatedAt = DateTime.UtcNow
            };
            await _stocks.AddAsync(stock, cancellationToken);
        }
        else
        {
            stock.Quantity = request.Quantity;
            stock.UpdatedAt = DateTime.UtcNow;
        }

        await _stocks.SaveChangesAsync(cancellationToken);

        var full = await _stocks.Query()
            .FirstAsync(s => s.StoreId == request.StoreId && s.ProductId == request.ProductId, cancellationToken);

        return new StockDto
        {
            StoreId = full.StoreId,
            ProductId = full.ProductId,
            StoreName = full.Store?.Name ?? string.Empty,
            ProductName = full.Product?.Name ?? string.Empty,
            Quantity = full.Quantity,
            UpdatedAt = full.UpdatedAt
        };
    }
}
