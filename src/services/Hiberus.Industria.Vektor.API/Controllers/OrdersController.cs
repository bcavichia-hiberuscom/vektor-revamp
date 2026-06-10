using ErrorOr;
using Hiberus.Industria.Vektor.Application.Common.Pagination;
using Hiberus.Industria.Vektor.Application.DTOs.Order;
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
    /// Retrieves all orders for a tenant with pagination and nested relations.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="pageNumber">The page number (default 1).</param>
    /// <param name="pageSize">The page size (default 20, max 100).</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A paginated list of orders with nested tenant and assignments.</returns>
    /// <response code="200">Returns the paginated list of orders.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<OrderDto>), StatusCodes.Status200OK)]
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
    /// Retrieves an order by its ID with nested relations.
    /// </summary>
    /// <param name="id">The ID of the order to retrieve.</param>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The order details with nested tenant and assignments.</returns>
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
        var order = await _service.GetByIdAsDto(id, tenantId, ct);
        if (order is null)
            return NotFound();

        return Ok(order);
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
        var orderDto = await _service.GetByIdAsDto(order.Id, tenantId, ct);
        
        return CreatedAtAction(
            nameof(GetById),
            new { id = order.Id, tenantId },
            orderDto
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
        var orderDto = await _service.GetByIdAsDto(order.Id, tenantId, ct);
        
        return Ok(orderDto);
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
        var orderDto = await _service.GetByIdAsDto(order.Id, tenantId, ct);
        
        return Ok(orderDto);
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
