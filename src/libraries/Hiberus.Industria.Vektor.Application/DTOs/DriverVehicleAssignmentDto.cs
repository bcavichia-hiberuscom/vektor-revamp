namespace Hiberus.Industria.Vektor.Application.DTOs;

public record DriverVehicleAssignmentDto(
    Guid Id,
    Guid TenantId,
    Guid DriverId,
    Guid VehicleId,
    DateTime AssignedAt,
    DateTime? UnassignedAt
);

public record CreateDriverVehicleAssignmentDto(Guid DriverId, Guid VehicleId);
