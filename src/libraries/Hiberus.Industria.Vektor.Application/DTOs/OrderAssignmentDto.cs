using Hiberus.Industria.Vektor.Domain.OrderAssignment;

namespace Hiberus.Industria.Vektor.Application.DTOs;

public record OrderAssignmentDto(
    Guid Id,
    Guid TenantId,
    Guid OrderId,
    Guid VehicleId,
    OrderAssignmentStatus Status,
    DateTime AssignedAt,
    DateTime? StartedAt,
    DateTime? CompletedAt,
    DateTime? ActualArrival,
    string? FailureReason,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record CreateOrderAssignmentDto(Guid OrderId, Guid VehicleId);

public record CompleteOrderAssignmentDto(DateTime ActualArrival);

public record FailOrderAssignmentDto(string Reason);
