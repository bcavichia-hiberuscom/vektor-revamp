using ErrorOr;
using Hiberus.Industria.Vektor.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Hiberus.Industria.Vektor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderAppService _service;

    public OrdersController(OrderAppService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retrieves all orders for a tenant.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A list of orders.</returns>
    /// <response code="200">Returns the list of orders.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll([FromQuery] Guid tenantId, CancellationToken ct)
    {
        var orders = await _service.GetAll(tenantId, ct);
        var response = orders.Select(o => new OrderDto(
            o.Id,
            o.TenantId,
            o.Label,
            o.Description,
            o.Latitude,
            o.Longitude,
            o.CustomerName,
            o.CustomerPhone,
            o.ExternalOrderId,
            o.Status.ToString()
        ));

        return Ok(response);
    }

    /// <summary>
    /// Retrieves an order by its ID.
    /// </summary>
    /// <param name="id">The ID of the order to retrieve.</param>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The order details.</returns>
    /// <response code="200">Returns the order.</response>
    /// <response code="404">Order with the specified ID was not found.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var order = await _service.GetById(id, tenantId, ct);
        if (order is null)
            return NotFound();

        return Ok(
            new OrderDto(
                order.Id,
                order.TenantId,
                order.Label,
                order.Description,
                order.Latitude,
                order.Longitude,
                order.CustomerName,
                order.CustomerPhone,
                order.ExternalOrderId,
                order.Status.ToString()
            )
        );
    }

    /// <summary>
    /// Creates a new order.
    /// </summary>
    /// <param name="dto">The order data.</param>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The created order.</returns>
    /// <response code="201">Order created successfully.</response>
    /// <response code="400">Validation error in the request data.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromBody] CreateOrderDto dto,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var result = await _service.Create(dto, tenantId, ct);

        if (result.IsError)
            return BadRequest(result.Errors);

        var order = result.Value;
        return CreatedAtAction(
            nameof(GetById),
            new { id = order.Id, tenantId },
            new OrderDto(
                order.Id,
                order.TenantId,
                order.Label,
                order.Description,
                order.Latitude,
                order.Longitude,
                order.CustomerName,
                order.CustomerPhone,
                order.ExternalOrderId,
                order.Status.ToString()
            )
        );
    }

    /// <summary>
    /// Updates an existing order.
    /// </summary>
    /// <param name="id">The ID of the order to update.</param>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="dto">The updated order data.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The updated order.</returns>
    /// <response code="200">Order updated successfully.</response>
    /// <response code="400">Validation error in the request data.</response>
    /// <response code="404">Order with the specified ID was not found.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromQuery] Guid tenantId,
        [FromBody] UpdateOrderDto dto,
        CancellationToken ct
    )
    {
        var result = await _service.Update(id, tenantId, dto, ct);

        if (result.IsError)
            return result.FirstError.Type == ErrorType.NotFound
                ? NotFound()
                : BadRequest(result.Errors);

        var order = result.Value;
        return Ok(
            new OrderDto(
                order.Id,
                order.TenantId,
                order.Label,
                order.Description,
                order.Latitude,
                order.Longitude,
                order.CustomerName,
                order.CustomerPhone,
                order.ExternalOrderId,
                order.Status.ToString()
            )
        );
    }

    /// <summary>
    /// Cancels an order.
    /// </summary>
    /// <param name="id">The ID of the order to cancel.</param>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The cancelled order.</returns>
    /// <response code="200">Order cancelled successfully.</response>
    /// <response code="404">Order with the specified ID was not found.</response>
    /// <response code="409">Order cannot be cancelled in its current state.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpPost("{id}/cancel")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Cancel(
        Guid id,
        [FromQuery] Guid tenantId,
        CancellationToken ct
    )
    {
        var result = await _service.Cancel(id, tenantId, ct);

        if (result.IsError)
            return result.FirstError.Type == ErrorType.NotFound
                ? NotFound()
                : Conflict(result.Errors);

        var order = result.Value;
        return Ok(
            new OrderDto(
                order.Id,
                order.TenantId,
                order.Label,
                order.Description,
                order.Latitude,
                order.Longitude,
                order.CustomerName,
                order.CustomerPhone,
                order.ExternalOrderId,
                order.Status.ToString()
            )
        );
    }

    /// <summary>
    /// Deletes an order by its ID.
    /// </summary>
    /// <param name="id">The ID of the order to delete.</param>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Order deleted successfully.</response>
    /// <response code="404">Order with the specified ID was not found.</response>
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
