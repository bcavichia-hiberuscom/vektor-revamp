using Hiberus.Industria.Vektor.Application.DTOs;
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
        var history = await _service.GetByTenant(tenantId, ct);
        return Ok(history.Select(ToDto));
    }

    [HttpGet("vehicle/{vehicleId}")]
    public async Task<IActionResult> GetByVehicle(
        Guid vehicleId,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var history = await _service.GetByVehicle(vehicleId, tenantId, ct);
        return Ok(history.Select(ToDto));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var entry = await _service.GetById(id, tenantId, ct);

        if (entry is null)
            return NotFound();

        return Ok(ToDto(entry));
    }

    private static RouteHistoryDto ToDto(Domain.RouteHistory.RouteHistory h) =>
        new(
            h.Id,
            h.TenantId,
            h.VehicleId,
            h.RoutePayload,
            h.AssociatedOrderIds,
            h.StartedAt,
            h.FinishedAt,
            h.FinishedAt - h.StartedAt
        );
}
