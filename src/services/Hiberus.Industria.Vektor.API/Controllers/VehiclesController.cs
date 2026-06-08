using Hiberus.Industria.Vektor.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Hiberus.Industria.Vektor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly VehicleAppService _service;

    public VehiclesController(VehicleAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid tenantId, CancellationToken ct)
    {
        var vehicles = await _service.GetAll(tenantId, ct);

        return Ok(
            vehicles.Select(v => new VehicleDto(
                v.Id,
                v.Label,
                v.LicensePlate,
                v.Brand,
                v.Model,
                v.Year,
                v.Type,
                v.Status
            ))
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var vehicle = await _service.GetById(id, tenantId, ct);

        if (vehicle is null)
            return NotFound();

        return Ok(
            new VehicleDto(
                vehicle.Id,
                vehicle.Label,
                vehicle.LicensePlate,
                vehicle.Brand,
                vehicle.Model,
                vehicle.Year,
                vehicle.Type,
                vehicle.Status
            )
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateVehicleDto dto,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var result = await _service.Create(dto, tenantId, ct);

        if (result.IsError)
            return BadRequest(result.Errors);

        var v = result.Value;

        return CreatedAtAction(
            nameof(GetById),
            new { id = v.Id, tenantId },
            new VehicleDto(
                v.Id,
                v.Label,
                v.LicensePlate,
                v.Brand,
                v.Model,
                v.Year,
                v.Type,
                v.Status
            )
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromQuery] Guid tenantId,
        [FromBody] UpdateVehicleDto dto,
        CancellationToken ct
    )
    {
        var result = await _service.Update(id, tenantId, dto, ct);

        if (result.IsError)
            return BadRequest(result.Errors);

        var v = result.Value;

        return Ok(
            new VehicleDto(
                v.Id,
                v.Label,
                v.LicensePlate,
                v.Brand,
                v.Model,
                v.Year,
                v.Type,
                v.Status
            )
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        Guid id,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        await _service.Delete(id, tenantId, ct);
        return NoContent();
    }
}
