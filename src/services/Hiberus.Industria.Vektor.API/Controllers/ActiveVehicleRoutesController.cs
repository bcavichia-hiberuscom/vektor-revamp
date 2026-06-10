using ErrorOr;
using Hiberus.Industria.Vektor.Application.DTOs.Route;
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
        var routes = await _service.GetAllAsDto(tenantId, ct);
        return Ok(routes);
    }

    [HttpGet("vehicle/{vehicleId}")]
    public async Task<IActionResult> GetByVehicle(
        Guid vehicleId,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var routes = await _service.GetByVehicleAsDto(vehicleId, tenantId, ct);
        return Ok(routes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var route = await _service.GetByIdAsDto(id, tenantId, ct);

        if (route is null)
            return NotFound();

        return Ok(route);
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
        var routeDto = await _service.GetByIdAsDto(r.Id, tenantId, ct);
        
        return CreatedAtAction(nameof(GetById), new { id = r.Id, tenantId }, routeDto);
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

        var routeDto = await _service.GetByIdAsDto(result.Value.Id, tenantId, ct);
        
        return Ok(routeDto);
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
        var historyDto = await _service.GetHistoryByIdAsDto(h.Id, tenantId, ct);
        
        return Ok(historyDto);
    }

}
