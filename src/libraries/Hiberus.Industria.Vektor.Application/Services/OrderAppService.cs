using ErrorOr;
using Hiberus.Industria.Vektor.Application.Common.Mappings;
using Hiberus.Industria.Vektor.Application.Common.Pagination;
using Hiberus.Industria.Vektor.Application.DTOs.Order;
using Hiberus.Industria.Vektor.Application.DTOs.Tenant;
using Hiberus.Industria.Vektor.Application.DTOs.OrderAssignment;
using Hiberus.Industria.Vektor.Application.DTOs.Vehicle;
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
    /// Default: 20 items/page, maximum: 100 items/page.
    /// </summary>
    public async Task<PagedResult<OrderDto>> GetAllPaginatedAsync(
        Guid tenantId,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken ct = default
    )
    {
        var (orders, totalCount) = await _repo.GetAllPaginatedAsync(tenantId, pageNumber, pageSize, ct);
        
        // Project entities to DTOs with nested relations
        var dtos = orders
            .Select(o => new OrderDto(
                o.Id,
                o.TenantId,
                o.Label,
                o.Description,
                o.Latitude,
                o.Longitude,
                o.CustomerName,
                o.CustomerPhone,
                o.ExternalOrderId,
                o.Status.ToString(),
                new TenantSummaryDto(o.Tenant.Id, o.Tenant.Name, o.Tenant.Slug),
                o.Assignments
                    .Select(a => new OrderAssignmentDto(
                        a.Id,
                        a.TenantId,
                        a.OrderId,
                        a.VehicleId,
                        a.Status.ToString(),
                        a.AssignedAt,
                        a.StartedAt,
                        a.CompletedAt,
                        a.ActualArrival,
                        a.FailureReason,
                        a.CreatedAt,
                        a.UpdatedAt,
                        new OrderSummaryDto(a.Order.Id, a.Order.Label, a.Order.Status.ToString(), a.Order.CustomerName),
                        new VehicleSummaryDto(
                            a.Vehicle.Id,
                            a.Vehicle.Label,
                            a.Vehicle.LicensePlate ?? string.Empty,
                            a.Vehicle.Brand ?? string.Empty,
                            a.Vehicle.Model ?? string.Empty,
                            a.Vehicle.Year ?? 0,
                            a.Vehicle.Type,
                            a.Vehicle.Status.ToString()
                        ),
                        new TenantSummaryDto(a.Tenant.Id, a.Tenant.Name, a.Tenant.Slug)
                    ))
                    .ToList()
            ))
            .ToList();

        return new PagedResult<OrderDto>(dtos, totalCount, pageNumber, pageSize);
    }

    /// <summary>
    /// Retrieves a single order by ID as DTO with nested relations.
    /// </summary>
    public async Task<OrderDto?> GetByIdAsDto(Guid id, Guid tenantId, CancellationToken ct)
    {
        var order = await _repo.GetByIdAsync(id, tenantId, ct);
        if (order is null)
            return null;

        return new OrderDto(
            order.Id,
            order.TenantId,
            order.Label,
            order.Description,
            order.Latitude,
            order.Longitude,
            order.CustomerName,
            order.CustomerPhone,
            order.ExternalOrderId,
            order.Status.ToString(),
            new TenantSummaryDto(order.Tenant.Id, order.Tenant.Name, order.Tenant.Slug),
            order.Assignments
                .Select(a => new OrderAssignmentDto(
                    a.Id,
                    a.TenantId,
                    a.OrderId,
                    a.VehicleId,
                    a.Status.ToString(),
                    a.AssignedAt,
                    a.StartedAt,
                    a.CompletedAt,
                    a.ActualArrival,
                    a.FailureReason,
                    a.CreatedAt,
                    a.UpdatedAt,
                    new OrderSummaryDto(a.Order.Id, a.Order.Label, a.Order.Status.ToString(), a.Order.CustomerName),
                    new VehicleSummaryDto(
                        a.Vehicle.Id,
                        a.Vehicle.Label,
                        a.Vehicle.LicensePlate ?? string.Empty,
                        a.Vehicle.Brand ?? string.Empty,
                        a.Vehicle.Model ?? string.Empty,
                        a.Vehicle.Year ?? 0,
                        a.Vehicle.Type,
                        a.Vehicle.Status.ToString()
                    ),
                    new TenantSummaryDto(a.Tenant.Id, a.Tenant.Name, a.Tenant.Slug)
                ))
                .ToList()
        );
    }

    public async Task<Order?> GetById(Guid id, Guid tenantId, CancellationToken ct) =>
        await _repo.GetByIdAsync(id, tenantId, ct);

    public async Task<ErrorOr<Order>> Create(
        CreateOrderDto dto,
        Guid tenantId,
        CancellationToken ct
    )
    {
        var result = Order.Create(
            tenantId,
            dto.Label,
            dto.Latitude,
            dto.Longitude,
            "system", // replace with user when auth is implemented
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

    public async Task<ErrorOr<Order>> Update(
        Guid id,
        Guid tenantId,
        UpdateOrderDto dto,
        CancellationToken ct
    )
    {
        var order = await _repo.GetByIdAsync(id, tenantId, ct);
        if (order is null)
            return Error.NotFound();

        var result = order.Update(
            dto.Label,
            dto.Latitude,
            dto.Longitude,
            "system", // replace with user when auth is implemented
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

    public async Task<ErrorOr<Order>> Cancel(Guid id, Guid tenantId, CancellationToken ct)
    {
        var order = await _repo.GetByIdAsync(id, tenantId, ct);
        if (order is null)
            return Error.NotFound();

        var result = order.Cancel("system"); // replace with user when auth is implemented
        if (result.IsError)
            return result.Errors;

        await _repo.UpdateAsync(order, ct);
        return order;
    }

    public async Task Delete(Guid id, Guid tenantId, CancellationToken ct) =>
        await _repo.DeleteAsync(id, tenantId, ct);
}
