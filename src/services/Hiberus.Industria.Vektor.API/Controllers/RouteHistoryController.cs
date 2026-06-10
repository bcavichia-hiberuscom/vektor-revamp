using Hiberus.Industria.Vektor.Application.DTOs.Route;
using Microsoft.AspNetCore.Mvc;

namespace Hiberus.Industria.Vektor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RouteHistoryController : ControllerBase
{
    private readonly RouteHistoryAppService _service;

    public RouteHistoryController(RouteHistoryAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetByTenant([FromQuery] Guid tenantId, CancellationToken ct)
    {
        var history = await _service.GetByTenantAsDto(tenantId, ct);
        return Ok(history);
    }

    [HttpGet("vehicle/{vehicleId}")]
    public async Task<IActionResult> GetByVehicle(
        Guid vehicleId,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var history = await _service.GetByVehicleAsDto(vehicleId, tenantId, ct);
        return Ok(history);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var entry = await _service.GetByIdAsDto(id, tenantId, ct);

        if (entry is null)
            return NotFound();

        return Ok(entry);
    }
}
