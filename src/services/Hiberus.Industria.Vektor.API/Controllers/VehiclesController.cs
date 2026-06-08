using Hiberus.Industria.Vektor.Application.DTOs;
using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.Vehicle;
using Microsoft.AspNetCore.Mvc;

namespace Hiberus.Industria.Vektor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleRepository _vehicleRepository;

    public VehiclesController(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    /// <summary>
    /// Retrieves all vehicles for a tenant.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A list of vehicles.</returns>
    /// <response code="200">Returns the list of vehicles.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<VehicleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll([FromQuery] Guid tenantId, CancellationToken ct)
    {
        var vehicles = await _vehicleRepository.GetAllAsync(tenantId, ct);
        var response = vehicles.Select(v => new VehicleDto(
            v.Id,
            v.Label,
            v.LicensePlate,
            v.Brand,
            v.Model,
            v.Year,
            v.Type,
            v.Status
        ));

        return Ok(response);
    }

    /// <summary>
    /// Retrieves a vehicle by its ID.
    /// </summary>
    /// <param name="id">The ID of the vehicle to retrieve.</param>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The vehicle details.</returns>
    /// <response code="200">Returns the vehicle.</response>
    /// <response code="404">Vehicle with the specified ID was not found.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id, tenantId, ct);
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

    /// <summary>
    /// Creates a new vehicle.
    /// </summary>
    /// <param name="dto">The vehicle data.</param>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The created vehicle.</returns>
    /// <response code="201">Vehicle created successfully.</response>
    /// <response code="400">Validation error in the request data.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpPost]
    [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromBody] CreateVehicleDto dto,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var result = Vehicle.Create(
            tenantId,
            dto.Label,
            dto.Type,
            "system",
            dto.LicensePlate,
            dto.Brand,
            dto.Model,
            dto.Year
        );

        if (result.IsError)
            return BadRequest(result.Errors);

        var vehicle = await _vehicleRepository.CreateAsync(result.Value, ct);

        return CreatedAtAction(
            nameof(GetById),
            new { id = vehicle.Id, tenantId },
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

    /// <summary>
    /// Updates an existing vehicle.
    /// </summary>
    /// <param name="id">The ID of the vehicle to update.</param>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="dto">The updated vehicle data.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The updated vehicle.</returns>
    /// <response code="200">Vehicle updated successfully.</response>
    /// <response code="400">Validation error in the request data.</response>
    /// <response code="404">Vehicle with the specified ID was not found.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromQuery] Guid tenantId,
        [FromBody] UpdateVehicleDto dto,
        CancellationToken ct
    )
    {
        var vehicle = await _vehicleRepository.UpdateAsync(id, tenantId, dto, ct);
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

    /// <summary>
    /// Deletes a vehicle by its ID.
    /// </summary>
    /// <param name="id">The ID of the vehicle to delete.</param>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Vehicle deleted successfully.</response>
    /// <response code="404">Vehicle with the specified ID was not found.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(
        Guid id,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        await _vehicleRepository.DeleteAsync(id, tenantId, ct);
        return NoContent();
    }
}
