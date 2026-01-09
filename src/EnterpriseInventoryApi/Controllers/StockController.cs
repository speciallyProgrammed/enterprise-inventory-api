using EnterpriseInventoryApi.Application.DTOs.Stock;
using EnterpriseInventoryApi.Application.Services;
using EnterpriseInventoryApi.Common.Pagination;
using EnterpriseInventoryApi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseInventoryApi.Controllers;

[ApiController]
[Route("api/stock")]
[Authorize]
public class StockController : ControllerBase
{
    private readonly IStockService _stocks;

    public StockController(IStockService stocks)
    {
        _stocks = stocks;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<StockDto>>> GetAll([FromQuery] Guid? storeId, [FromQuery] Guid? productId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var result = await _stocks.GetAsync(storeId, productId, page, pageSize, cancellationToken);
        return Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<StockDto>> Upsert([FromBody] StockUpsertRequest request, CancellationToken cancellationToken)
    {
        var result = await _stocks.UpsertAsync(request, cancellationToken);
        return Ok(result);
    }
}
