using EnterpriseInventoryApi.Application.DTOs.Stores;
using EnterpriseInventoryApi.Application.Services;
using EnterpriseInventoryApi.Common.Pagination;
using EnterpriseInventoryApi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseInventoryApi.Controllers;

[ApiController]
[Route("api/stores")]
[Authorize]
public class StoresController : ControllerBase
{
    private readonly IStoreService _stores;

    public StoresController(IStoreService stores)
    {
        _stores = stores;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<StoreDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, CancellationToken cancellationToken = default)
    {
        var result = await _stores.GetAsync(page, pageSize, search, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<StoreDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _stores.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<StoreDto>> Create([FromBody] StoreCreateRequest request, CancellationToken cancellationToken)
    {
        var result = await _stores.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<StoreDto>> Update(Guid id, [FromBody] StoreUpdateRequest request, CancellationToken cancellationToken)
    {
        var result = await _stores.UpdateAsync(id, request, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _stores.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
