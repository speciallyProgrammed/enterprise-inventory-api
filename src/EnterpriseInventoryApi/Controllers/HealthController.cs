using Microsoft.AspNetCore.Mvc;

namespace EnterpriseInventoryApi.Controllers;

[ApiController]
public class HealthController : ControllerBase
{
    [HttpGet("/health")]
    public IActionResult Get()
    {
        return Ok(new { status = "ok" });
    }
}
