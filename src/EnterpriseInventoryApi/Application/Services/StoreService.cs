using EnterpriseInventoryApi.Application.DTOs.Stores;
using EnterpriseInventoryApi.Common.Errors;
using EnterpriseInventoryApi.Common.Pagination;
using EnterpriseInventoryApi.Domain.Entities;
using EnterpriseInventoryApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseInventoryApi.Application.Services;

public class StoreService : IStoreService
{
    private readonly IStoreRepository _stores;

    public StoreService(IStoreRepository stores)
    {
        _stores = stores;
    }

    public async Task<PagedResult<StoreDto>> GetAsync(int page, int pageSize, string? search, CancellationToken cancellationToken)
    {
        var query = _stores.Query();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(s => s.Name.Contains(term) || s.Code.Contains(term));
        }

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(s => s.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new StoreDto
            {
                Id = s.Id,
                Code = s.Code,
                Name = s.Name,
                CreatedAt = s.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<StoreDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = total
        };
    }

    public async Task<StoreDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var store = await _stores.GetByIdAsync(id, cancellationToken);
        if (store == null)
        {
            throw new ApiException(ErrorCodes.NotFound, "Store not found", 404);
        }

        return new StoreDto
        {
            Id = store.Id,
            Code = store.Code,
            Name = store.Name,
            CreatedAt = store.CreatedAt
        };
    }

    public async Task<StoreDto> CreateAsync(StoreCreateRequest request, CancellationToken cancellationToken)
    {
        var exists = await _stores.Query().AnyAsync(s => s.Code == request.Code, cancellationToken);
        if (exists)
        {
            throw new ApiException(ErrorCodes.Conflict, "Store code already exists", 409);
        }

        var store = new Store
        {
            Code = request.Code,
            Name = request.Name
        };

        await _stores.AddAsync(store, cancellationToken);
        await _stores.SaveChangesAsync(cancellationToken);

        return new StoreDto
        {
            Id = store.Id,
            Code = store.Code,
            Name = store.Name,
            CreatedAt = store.CreatedAt
        };
    }

    public async Task<StoreDto> UpdateAsync(Guid id, StoreUpdateRequest request, CancellationToken cancellationToken)
    {
        var store = await _stores.GetByIdAsync(id, cancellationToken);
        if (store == null)
        {
            throw new ApiException(ErrorCodes.NotFound, "Store not found", 404);
        }

        var codeExists = await _stores.Query().AnyAsync(s => s.Code == request.Code && s.Id != id, cancellationToken);
        if (codeExists)
        {
            throw new ApiException(ErrorCodes.Conflict, "Store code already exists", 409);
        }

        store.Code = request.Code;
        store.Name = request.Name;

        await _stores.SaveChangesAsync(cancellationToken);

        return new StoreDto
        {
            Id = store.Id,
            Code = store.Code,
            Name = store.Name,
            CreatedAt = store.CreatedAt
        };
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var store = await _stores.GetByIdAsync(id, cancellationToken);
        if (store == null)
        {
            throw new ApiException(ErrorCodes.NotFound, "Store not found", 404);
        }

        _stores.Remove(store);
        await _stores.SaveChangesAsync(cancellationToken);
    }
}
