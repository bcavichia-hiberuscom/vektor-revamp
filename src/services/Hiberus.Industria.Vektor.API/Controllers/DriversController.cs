using ErrorOr;
using Hiberus.Industria.Vektor.Application.Common.Pagination;
using Hiberus.Industria.Vektor.Application.DTOs.Driver;
using Microsoft.AspNetCore.Mvc;

namespace Hiberus.Industria.Vektor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DriversController : ControllerBase
{
    private readonly DriverAppService _service;

    public DriversController(DriverAppService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retrieves all drivers for a tenant with pagination and nested relations.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="pageNumber">The page number (default 1).</param>
    /// <param name="pageSize">The page size (default 20, max 100).</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A paginated list of drivers with nested tenant and assignments.</returns>
    /// <response code="200">Returns the paginated list of drivers.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<DriverDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    /// <summary>
    /// Retrieves a driver by its ID with nested relations.
    /// </summary>
    /// <param name="id">The ID of the driver to retrieve.</param>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The driver details with nested tenant and assignments.</returns>
    /// <response code="200">Returns the driver.</response>
    /// <response code="404">Driver with the specified ID was not found.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(DriverDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var driver = await _service.GetByIdAsDto(id, tenantId, ct);
        if (driver is null)
            return NotFound();

        return Ok(driver);
    }

    /// <summary>
    /// Creates a new driver.
    /// </summary>
    /// <param name="dto">The driver data.</param>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The created driver.</returns>
    /// <response code="201">Driver created successfully.</response>
    /// <response code="400">Validation error in the request data.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpPost]
    [ProducesResponseType(typeof(DriverDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromBody] CreateDriverDto dto,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var result = await _service.Create(dto, tenantId, ct);

        if (result.IsError)
            return BadRequest(result.Errors);

        var driver = result.Value;
        var driverDto = await _service.GetByIdAsDto(driver.Id, tenantId, ct);

        return CreatedAtAction(nameof(GetById), new { id = driver.Id, tenantId }, driverDto);
    }

    /// <summary>
    /// Updates an existing driver.
    /// </summary>
    /// <param name="id">The ID of the driver to update.</param>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="dto">The updated driver data.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The updated driver.</returns>
    /// <response code="200">Driver updated successfully.</response>
    /// <response code="400">Validation error in the request data.</response>
    /// <response code="404">Driver with the specified ID was not found.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(DriverDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromQuery] Guid tenantId,
        [FromBody] UpdateDriverDto dto,
        CancellationToken ct
    )
    {
        var result = await _service.Update(id, tenantId, dto, ct);

        if (result.IsError)
            return result.FirstError.Type == ErrorType.NotFound
                ? NotFound()
                : BadRequest(result.Errors);

        var driver = result.Value;
        var driverDto = await _service.GetByIdAsDto(driver.Id, tenantId, ct);

        return Ok(driverDto);
    }

    /// <summary>
    /// Deletes a driver by its ID.
    /// </summary>
    /// <param name="id">The ID of the driver to delete.</param>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Driver deleted successfully.</response>
    /// <response code="404">Driver with the specified ID was not found.</response>
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
        await _service.Delete(id, tenantId, ct);
        return NoContent();
    }
}
