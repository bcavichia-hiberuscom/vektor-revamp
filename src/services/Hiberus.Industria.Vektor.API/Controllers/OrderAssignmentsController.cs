using ErrorOr;
using Hiberus.Industria.Vektor.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Hiberus.Industria.Vektor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderAssignmentsController : ControllerBase
{
    private readonly OrderAssignmentAppService _service;

    public OrderAssignmentsController(OrderAssignmentAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetByTenant([FromQuery] Guid tenantId, CancellationToken ct)
    {
        var assignments = await _service.GetByTenant(tenantId, ct);
        return Ok(assignments.Select(ToDto));
    }

    [HttpGet("order/{orderId}")]
    public async Task<IActionResult> GetByOrder(
        Guid orderId,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var assignments = await _service.GetByOrder(orderId, tenantId, ct);
        return Ok(assignments.Select(ToDto));
    }

    [HttpGet("vehicle/{vehicleId}")]
    public async Task<IActionResult> GetByVehicle(
        Guid vehicleId,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var assignments = await _service.GetByVehicle(vehicleId, tenantId, ct);
        return Ok(assignments.Select(ToDto));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var assignment = await _service.GetById(id, tenantId, ct);

        if (assignment is null)
            return NotFound();

        return Ok(ToDto(assignment));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateOrderAssignmentDto dto,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var result = await _service.Create(dto, tenantId, ct);

        if (result.IsError)
            return BadRequest(result.Errors);

        var a = result.Value;
        return CreatedAtAction(nameof(GetById), new { id = a.Id, tenantId }, ToDto(a));
    }

    [HttpPost("{id}/start")]
    public async Task<IActionResult> Start(Guid id, [FromQuery] Guid tenantId, CancellationToken ct)
    {
        var result = await _service.Start(id, tenantId, ct);

        if (result.IsError)
            return result.FirstError.Type == ErrorType.NotFound
                ? NotFound()
                : BadRequest(result.Errors);

        return Ok(ToDto(result.Value));
    }

    [HttpPost("{id}/complete")]
    public async Task<IActionResult> Complete(
        Guid id,
        [FromQuery] Guid tenantId,
        [FromBody] CompleteOrderAssignmentDto dto,
        CancellationToken ct
    )
    {
        var result = await _service.Complete(id, tenantId, dto, ct);

        if (result.IsError)
            return result.FirstError.Type == ErrorType.NotFound
                ? NotFound()
                : BadRequest(result.Errors);

        return Ok(ToDto(result.Value));
    }

    [HttpPost("{id}/fail")]
    public async Task<IActionResult> Fail(
        Guid id,
        [FromQuery] Guid tenantId,
        [FromBody] FailOrderAssignmentDto dto,
        CancellationToken ct
    )
    {
        var result = await _service.Fail(id, tenantId, dto, ct);

        if (result.IsError)
            return result.FirstError.Type == ErrorType.NotFound
                ? NotFound()
                : BadRequest(result.Errors);

        return Ok(ToDto(result.Value));
    }

    private static OrderAssignmentDto ToDto(Domain.OrderAssignment.OrderAssignment a) =>
        new(
            a.Id,
            a.TenantId,
            a.OrderId,
            a.VehicleId,
            a.Status,
            a.AssignedAt,
            a.StartedAt,
            a.CompletedAt,
            a.ActualArrival,
            a.FailureReason,
            a.CreatedAt,
            a.UpdatedAt
        );
}
