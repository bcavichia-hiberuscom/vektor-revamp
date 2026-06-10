using ErrorOr;
using Hiberus.Industria.Vektor.Application.Common.Pagination;
using Hiberus.Industria.Vektor.Application.DTOs.Order;
using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.Order;

public class OrderAppService
{
    private readonly IOrderRepository _repo;

    public OrderAppService(IOrderRepository repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// Retrieves orders with pagination, returning DTOs with nested relations.
    /// Uses database-level projections to minimize data transfer and N+1 queries.
    /// Default: 20 items/page, maximum: 100 items/page.
    /// </summary>
    public async Task<PagedResult<OrderDto>> GetAllPaginatedAsync(
        Guid tenantId,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken ct = default
    )
    {
        // Repository returns DTOs already projected at database level
        var (dtos, totalCount) = await _repo.GetAllPaginatedAsDtoAsync(
            tenantId,
            pageNumber,
            pageSize,
            ct
        );

        // Convert IEnumerable to List (which implements IReadOnlyCollection)
        return new PagedResult<OrderDto>(dtos.ToList(), totalCount, pageNumber, pageSize);
    }

    /// <summary>
    /// Retrieves a single order by ID as DTO with nested relations.
    /// Uses database-level projection for optimal performance.
    /// </summary>
    public async Task<OrderDto?> GetByIdAsDto(Guid id, Guid tenantId, CancellationToken ct) =>
        await _repo.GetByIdAsDtoAsync(id, tenantId, ct);

    // ==================== CRUD Operations ====================

    /// <summary>
    /// Retrieves a single order entity by ID for CRUD operations.
    /// Returns raw entity for mutation operations (Update, Delete).
    /// For read-only scenarios with DTOs, use GetByIdAsDto() instead.
    /// </summary>
    public async Task<Order?> GetById(Guid id, Guid tenantId, CancellationToken ct) =>
        await _repo.GetByIdAsync(id, tenantId, ct);

    /// <summary>
    /// Creates a new order with validation at the domain level.
    /// Validates business rules before persisting to the database.
    /// </summary>
    /// <returns>
    /// ErrorOr result containing the created order or domain validation errors.
    /// </returns>
    public async Task<ErrorOr<Order>> Create(
        CreateOrderDto dto,
        Guid tenantId,
        CancellationToken ct
    )
    {
        // Delegate to domain aggregate root for business logic validation
        var result = Order.Create(
            tenantId,
            dto.Label,
            dto.Latitude,
            dto.Longitude,
            "system", // TODO: Replace with authenticated user when auth is implemented
            dto.ExternalOrderId,
            dto.Description,
            dto.CustomerName,
            dto.CustomerPhone
        );

        if (result.IsError)
            return result.Errors;

        var order = await _repo.CreateAsync(result.Value, ct);
        return order;
    }

    /// <summary>
    /// Updates an existing order with validation at the domain level.
    /// Validates business rules and tenant isolation before persisting.
    /// </summary>
    /// <returns>
    /// ErrorOr result containing the updated order, not-found error, or domain validation errors.
    /// </returns>
    public async Task<ErrorOr<Order>> Update(
        Guid id,
        Guid tenantId,
        UpdateOrderDto dto,
        CancellationToken ct
    )
    {
        // Retrieve entity for mutation - uses raw entity repository method
        var order = await _repo.GetByIdAsync(id, tenantId, ct);
        if (order is null)
            return Error.NotFound();

        // Delegate to domain aggregate root for business logic validation
        var result = order.Update(
            dto.Label,
            dto.Latitude,
            dto.Longitude,
            "system", // TODO: Replace with authenticated user when auth is implemented
            dto.ExternalOrderId,
            dto.Description,
            dto.CustomerName,
            dto.CustomerPhone
        );

        if (result.IsError)
            return result.Errors;

        await _repo.UpdateAsync(order, ct);
        return order;
    }

    /// <summary>
    /// Cancels an existing order by transitioning its status to Cancelled.
    /// Only affects orders in valid states for cancellation.
    /// </summary>
    /// <returns>
    /// ErrorOr result containing the cancelled order, not-found error, or domain validation errors.
    /// </returns>
    public async Task<ErrorOr<Order>> Cancel(Guid id, Guid tenantId, CancellationToken ct)
    {
        // Retrieve entity for mutation - uses raw entity repository method
        var order = await _repo.GetByIdAsync(id, tenantId, ct);
        if (order is null)
            return Error.NotFound();

        // Delegate to domain aggregate root for business logic validation
        var result = order.Cancel("system"); // TODO: Replace with authenticated user when auth is implemented
        if (result.IsError)
            return result.Errors;

        await _repo.UpdateAsync(order, ct);
        return order;
    }

    /// <summary>
    /// Deletes an order from the database with tenant isolation validation.
    /// </summary>
    public async Task Delete(Guid id, Guid tenantId, CancellationToken ct) =>
        await _repo.DeleteAsync(id, tenantId, ct);
}
