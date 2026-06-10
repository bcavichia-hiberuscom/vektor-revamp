using ErrorOr;
using Hiberus.Industria.Vektor.Application.Common.Mappings;
using Hiberus.Industria.Vektor.Application.DTOs.OrderAssignment;
using Hiberus.Industria.Vektor.Application.DTOs.Order;
using Hiberus.Industria.Vektor.Application.DTOs.Vehicle;
using Hiberus.Industria.Vektor.Application.DTOs.Tenant;
using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.OrderAssignment;

public class OrderAssignmentAppService
{
    private readonly IOrderAssignmentRepository _repo;

    public OrderAssignmentAppService(IOrderAssignmentRepository repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// Retrieves all order assignments for a tenant as DTOs with nested relations.
    /// </summary>
    public async Task<IEnumerable<OrderAssignmentDto>> GetByTenantAsDto(
        Guid tenantId,
        CancellationToken ct
    )
    {
        var assignments = await _repo.GetByTenantAsync(tenantId, ct);
        
        return assignments
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
            .ToList();
    }

    /// <summary>
    /// Retrieves all order assignments for an order as DTOs with nested relations.
    /// </summary>
    public async Task<IEnumerable<OrderAssignmentDto>> GetByOrderAsDto(
        Guid orderId,
        Guid tenantId,
        CancellationToken ct
    )
    {
        var assignments = await _repo.GetByOrderAsync(orderId, tenantId, ct);
        
        return assignments
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
            .ToList();
    }

    /// <summary>
    /// Retrieves all order assignments for a vehicle as DTOs with nested relations.
    /// </summary>
    public async Task<IEnumerable<OrderAssignmentDto>> GetByVehicleAsDto(
        Guid vehicleId,
        Guid tenantId,
        CancellationToken ct
    )
    {
        var assignments = await _repo.GetByVehicleAsync(vehicleId, tenantId, ct);
        
        return assignments
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
            .ToList();
    }

    /// <summary>
    /// Retrieves a single order assignment by ID as DTO with nested relations.
    /// </summary>
    public async Task<OrderAssignmentDto?> GetByIdAsDto(Guid id, Guid tenantId, CancellationToken ct)
    {
        var assignment = await _repo.GetByIdAsync(id, tenantId, ct);
        if (assignment is null)
            return null;

        return new OrderAssignmentDto(
            assignment.Id,
            assignment.TenantId,
            assignment.OrderId,
            assignment.VehicleId,
            assignment.Status.ToString(),
            assignment.AssignedAt,
            assignment.StartedAt,
            assignment.CompletedAt,
            assignment.ActualArrival,
            assignment.FailureReason,
            assignment.CreatedAt,
            assignment.UpdatedAt,
            new OrderSummaryDto(assignment.Order.Id, assignment.Order.Label, assignment.Order.Status.ToString(), assignment.Order.CustomerName),
            new VehicleSummaryDto(
                assignment.Vehicle.Id,
                assignment.Vehicle.Label,
                assignment.Vehicle.LicensePlate ?? string.Empty,
                assignment.Vehicle.Brand ?? string.Empty,
                assignment.Vehicle.Model ?? string.Empty,
                assignment.Vehicle.Year ?? 0,
                assignment.Vehicle.Type,
                assignment.Vehicle.Status.ToString()
            ),
            new TenantSummaryDto(assignment.Tenant.Id, assignment.Tenant.Name, assignment.Tenant.Slug)
        );
    }

    public async Task<IEnumerable<OrderAssignment>> GetByTenant(
        Guid tenantId,
        CancellationToken ct
    ) => await _repo.GetByTenantAsync(tenantId, ct);

    public async Task<IEnumerable<OrderAssignment>> GetByOrder(
        Guid orderId,
        Guid tenantId,
        CancellationToken ct
    ) => await _repo.GetByOrderAsync(orderId, tenantId, ct);

    public async Task<IEnumerable<OrderAssignment>> GetByVehicle(
        Guid vehicleId,
        Guid tenantId,
        CancellationToken ct
    ) => await _repo.GetByVehicleAsync(vehicleId, tenantId, ct);

    public async Task<ErrorOr<OrderAssignment>> Create(
        CreateOrderAssignmentDto dto,
        Guid tenantId,
        CancellationToken ct
    )
    {
        var result = OrderAssignment.Create(
            tenantId,
            dto.OrderId,
            dto.VehicleId,
            "system" // TODO: replace with authenticated user
        );

        if (result.IsError)
            return result.Errors;

        return await _repo.CreateAsync(result.Value, ct);
    }

    public async Task<ErrorOr<OrderAssignment>> Start(Guid id, Guid tenantId, CancellationToken ct)
    {
        var assignment = await _repo.GetByIdAsync(id, tenantId, ct);
        if (assignment is null)
            return Error.NotFound("OrderAssignment.NotFound", "Assignment not found");

        var result = assignment.Start("system"); // TODO: replace with authenticated user
        if (result.IsError)
            return result.Errors;

        await _repo.UpdateAsync(assignment, ct);
        return assignment;
    }

    public async Task<ErrorOr<OrderAssignment>> Complete(
        Guid id,
        Guid tenantId,
        CompleteOrderAssignmentDto dto,
        CancellationToken ct
    )
    {
        var assignment = await _repo.GetByIdAsync(id, tenantId, ct);
        if (assignment is null)
            return Error.NotFound("OrderAssignment.NotFound", "Assignment not found");

        var result = assignment.Complete(dto.ActualArrival, "system"); // TODO: replace with authenticated user
        if (result.IsError)
            return result.Errors;

        await _repo.UpdateAsync(assignment, ct);
        return assignment;
    }

    public async Task<ErrorOr<OrderAssignment>> Fail(
        Guid id,
        Guid tenantId,
        FailOrderAssignmentDto dto,
        CancellationToken ct
    )
    {
        var assignment = await _repo.GetByIdAsync(id, tenantId, ct);
        if (assignment is null)
            return Error.NotFound("OrderAssignment.NotFound", "Assignment not found");

        var result = assignment.Fail(dto.Reason, "system"); // TODO: replace with authenticated user
        if (result.IsError)
            return result.Errors;

        await _repo.UpdateAsync(assignment, ct);
        return assignment;
    }
}
