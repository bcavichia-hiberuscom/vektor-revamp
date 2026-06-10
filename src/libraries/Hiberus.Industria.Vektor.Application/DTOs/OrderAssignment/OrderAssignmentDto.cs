using Hiberus.Industria.Vektor.Application.DTOs.Order;
using Hiberus.Industria.Vektor.Application.DTOs.Tenant;
using Hiberus.Industria.Vektor.Application.DTOs.Vehicle;
using Hiberus.Industria.Vektor.Domain.OrderAssignment;

namespace Hiberus.Industria.Vektor.Application.DTOs.OrderAssignment;

/// <summary>
/// Complete order assignment information with nested order, vehicle, and tenant.
/// Includes 1-level nesting to avoid circular references.
/// </summary>
public record OrderAssignmentDto(
    Guid Id,
    Guid TenantId,
    Guid OrderId,
    Guid VehicleId,
    string Status,
    DateTime AssignedAt,
    DateTime? StartedAt,
    DateTime? CompletedAt,
    DateTime? ActualArrival,
    string? FailureReason,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    OrderSummaryDto? Order,
    VehicleSummaryDto? Vehicle,
    TenantSummaryDto? Tenant
);

public record CreateOrderAssignmentDto(Guid OrderId, Guid VehicleId);

public record CompleteOrderAssignmentDto(DateTime ActualArrival);

public record FailOrderAssignmentDto(string Reason);
