using ErrorOr;
using Hiberus.Industria.Vektor.Application.DTOs.DriverVehicleAssignment;
using Microsoft.AspNetCore.Mvc;

namespace Hiberus.Industria.Vektor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DriverVehicleAssignmentsController : ControllerBase
{
    private readonly DriverVehicleAssignmentAppService _service;

    public DriverVehicleAssignmentsController(DriverVehicleAssignmentAppService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DriverVehicleAssignmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] Guid tenantId, CancellationToken ct)
    {
        var assignments = await _service.GetAllAsDto(tenantId, ct);
        return Ok(assignments);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(DriverVehicleAssignmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var assignment = await _service.GetByIdAsDto(id, tenantId, ct);

        if (assignment is null)
            return NotFound();

        return Ok(assignment);
    }

    [HttpGet("driver/{driverId}")]
    [ProducesResponseType(typeof(IEnumerable<DriverVehicleAssignmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByDriver(
        Guid driverId,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var assignments = await _service.GetByDriverAsDto(driverId, tenantId, ct);
        return Ok(assignments);
    }

    [HttpGet("vehicle/{vehicleId}")]
    [ProducesResponseType(typeof(IEnumerable<DriverVehicleAssignmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByVehicle(
        Guid vehicleId,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var assignments = await _service.GetByVehicleAsDto(vehicleId, tenantId, ct);
        return Ok(assignments);
    }

    [HttpPost]
    [ProducesResponseType(typeof(DriverVehicleAssignmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateDriverVehicleAssignmentDto dto,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var result = await _service.Create(dto, tenantId, ct);

        if (result.IsError)
            return BadRequest(result.Errors);

        var assignment = result.Value;
        var assignmentDto = await _service.GetByIdAsDto(assignment.Id, tenantId, ct);
        
        return CreatedAtAction(
            nameof(GetById),
            new { id = assignment.Id, tenantId },
            assignmentDto
        );
    }

    [HttpPost("{id}/unassign")]
    [ProducesResponseType(typeof(DriverVehicleAssignmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Unassign(
        Guid id,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var result = await _service.Unassign(id, tenantId, ct);

        if (result.IsError)
        {
            return result.FirstError.Type == ErrorType.NotFound
                ? NotFound()
                : BadRequest(result.Errors);
        }

        var assignment = result.Value;
        var assignmentDto = await _service.GetByIdAsDto(assignment.Id, tenantId, ct);
        
        return Ok(assignmentDto);
    }
}
