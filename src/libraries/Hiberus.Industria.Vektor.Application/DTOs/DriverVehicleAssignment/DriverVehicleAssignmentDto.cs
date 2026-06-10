using Hiberus.Industria.Vektor.Application.DTOs.Driver;
using Hiberus.Industria.Vektor.Application.DTOs.Tenant;
using Hiberus.Industria.Vektor.Application.DTOs.Vehicle;

namespace Hiberus.Industria.Vektor.Application.DTOs.DriverVehicleAssignment;

/// <summary>
/// Complete driver-vehicle assignment information with nested driver, vehicle, and tenant.
/// Includes 1-level nesting to avoid circular references.
/// </summary>
public record DriverVehicleAssignmentDto(
    Guid Id,
    Guid TenantId,
    Guid DriverId,
    Guid VehicleId,
    DateTime AssignedAt,
    DateTime? UnassignedAt,
    DriverSummaryDto Driver,
    VehicleSummaryDto Vehicle,
    TenantSummaryDto Tenant
);

public record CreateDriverVehicleAssignmentDto(Guid DriverId, Guid VehicleId);
