using EnterpriseInventoryApi.Application.DTOs.Products;
using EnterpriseInventoryApi.Common.Errors;
using EnterpriseInventoryApi.Common.Pagination;
using EnterpriseInventoryApi.Domain.Entities;
using EnterpriseInventoryApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseInventoryApi.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _products;

    public ProductService(IProductRepository products)
    {
        _products = products;
    }

    public async Task<PagedResult<ProductDto>> GetAsync(int page, int pageSize, string? search, CancellationToken cancellationToken)
    {
        var query = _products.Query();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(p => p.Name.Contains(term) || p.Sku.Contains(term));
        }

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Sku = p.Sku,
                Name = p.Name,
                Price = p.Price,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<ProductDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = total
        };
    }

    public async Task<ProductDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = await _products.GetByIdAsync(id, cancellationToken);
        if (product == null)
        {
            throw new ApiException(ErrorCodes.NotFound, "Product not found", 404);
        }

        return new ProductDto
        {
            Id = product.Id,
            Sku = product.Sku,
            Name = product.Name,
            Price = product.Price,
            CreatedAt = product.CreatedAt
        };
    }

    public async Task<ProductDto> CreateAsync(ProductCreateRequest request, CancellationToken cancellationToken)
    {
        var exists = await _products.Query().AnyAsync(p => p.Sku == request.Sku, cancellationToken);
        if (exists)
        {
            throw new ApiException(ErrorCodes.Conflict, "Product SKU already exists", 409);
        }

        var product = new Product
        {
            Sku = request.Sku,
            Name = request.Name,
            Price = request.Price
        };

        await _products.AddAsync(product, cancellationToken);
        await _products.SaveChangesAsync(cancellationToken);

        return new ProductDto
        {
            Id = product.Id,
            Sku = product.Sku,
            Name = product.Name,
            Price = product.Price,
            CreatedAt = product.CreatedAt
        };
    }

    public async Task<ProductDto> UpdateAsync(Guid id, ProductUpdateRequest request, CancellationToken cancellationToken)
    {
        var product = await _products.GetByIdAsync(id, cancellationToken);
        if (product == null)
        {
            throw new ApiException(ErrorCodes.NotFound, "Product not found", 404);
        }

        var skuExists = await _products.Query().AnyAsync(p => p.Sku == request.Sku && p.Id != id, cancellationToken);
        if (skuExists)
        {
            throw new ApiException(ErrorCodes.Conflict, "Product SKU already exists", 409);
        }

        product.Sku = request.Sku;
        product.Name = request.Name;
        product.Price = request.Price;

        await _products.SaveChangesAsync(cancellationToken);

        return new ProductDto
        {
            Id = product.Id,
            Sku = product.Sku,
            Name = product.Name,
            Price = product.Price,
            CreatedAt = product.CreatedAt
        };
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = await _products.GetByIdAsync(id, cancellationToken);
        if (product == null)
        {
            throw new ApiException(ErrorCodes.NotFound, "Product not found", 404);
        }

        _products.Remove(product);
        await _products.SaveChangesAsync(cancellationToken);
    }
}
