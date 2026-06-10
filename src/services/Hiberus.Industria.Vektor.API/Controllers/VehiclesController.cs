using Hiberus.Industria.Vektor.Application.DTOs.Vehicle;
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
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid tenantId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default
    )
    {
        var result = await _service.GetAllPaginatedAsync(tenantId, pageNumber, pageSize, ct);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var vehicle = await _service.GetByIdAsDto(id, tenantId, ct);

        if (vehicle is null)
            return NotFound();

        return Ok(vehicle);
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
        var vehicleDto = await _service.GetByIdAsDto(v.Id, tenantId, ct);
        
        return CreatedAtAction(
            nameof(GetById),
            new { id = v.Id, tenantId },
            vehicleDto
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
        var vehicleDto = await _service.GetByIdAsDto(v.Id, tenantId, ct);
        
        return Ok(vehicleDto);
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
