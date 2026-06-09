using ErrorOr;
using Hiberus.Industria.Vektor.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Hiberus.Industria.Vektor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActiveVehicleRoutesController : ControllerBase
{
    private readonly ActiveVehicleRouteAppService _service;

    public ActiveVehicleRoutesController(ActiveVehicleRouteAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid tenantId, CancellationToken ct)
    {
        var routes = await _service.GetAll(tenantId, ct);
        return Ok(routes.Select(ToDto));
    }

    [HttpGet("vehicle/{vehicleId}")]
    public async Task<IActionResult> GetByVehicle(
        Guid vehicleId,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var routes = await _service.GetByVehicle(vehicleId, tenantId, ct);
        return Ok(routes.Select(ToDto));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var route = await _service.GetById(id, tenantId, ct);

        if (route is null)
            return NotFound();

        return Ok(ToDto(route));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateActiveVehicleRouteDto dto,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var result = await _service.Create(dto, tenantId, ct);

        if (result.IsError)
            return BadRequest(result.Errors);

        var r = result.Value;
        return CreatedAtAction(nameof(GetById), new { id = r.Id, tenantId }, ToDto(r));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromQuery] Guid tenantId,
        [FromBody] UpdateActiveVehicleRouteDto dto,
        CancellationToken ct
    )
    {
        var result = await _service.Update(id, tenantId, dto, ct);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(ToDto(result.Value));
    }

    // POST /api/active-vehicle-routes/{id}/complete
    // Completa la ruta: la mueve a historial y la elimina del mapa
    [HttpPost("{id}/complete")]
    public async Task<IActionResult> Complete(
        Guid id,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var result = await _service.Complete(id, tenantId, ct);

        if (result.IsError)
            return result.FirstError.Type == ErrorType.NotFound
                ? NotFound()
                : BadRequest(result.Errors);

        var h = result.Value;
        return Ok(
            new RouteHistoryDto(
                h.Id,
                h.TenantId,
                h.VehicleId,
                h.RoutePayload,
                h.AssociatedOrderIds,
                h.StartedAt,
                h.FinishedAt,
                h.FinishedAt - h.StartedAt
            )
        );
    }

    private static ActiveVehicleRouteDto ToDto(Domain.ActiveVehicleRoute.ActiveVehicleRoute r) =>
        new(
            r.Id,
            r.TenantId,
            r.VehicleId,
            r.RoutePayload,
            r.AssociatedOrderIds,
            r.StartedAt,
            r.CreatedAt
        );
}
