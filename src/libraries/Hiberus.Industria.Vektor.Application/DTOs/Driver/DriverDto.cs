using Hiberus.Industria.Vektor.Application.DTOs.DriverVehicleAssignment;
using Hiberus.Industria.Vektor.Application.DTOs.Tenant;
using Hiberus.Industria.Vektor.Domain.Driver;

namespace Hiberus.Industria.Vektor.Application.DTOs.Driver;

/// <summary>
/// Complete driver information with nested tenant and vehicle assignments.
/// Includes 1-level nesting to avoid circular references.
/// </summary>
public record DriverDto(
    Guid Id,
    Guid TenantId,
    string Name,
    string? PhoneNumber,
    LicenseType LicenseType,
    string? LicenseNumber,
    DateTime? LicenseExpiryDate,
    bool IsAvailable,
    string? ImageUrl,
    int? WorkdayStartTime,
    int? WorkdayEndTime,
    string Timezone,
    TenantSummaryDto Tenant,
    IReadOnlyCollection<DriverVehicleAssignmentDto> VehicleAssignments
);

public record CreateDriverDto(
    string Name,
    string? PhoneNumber,
    LicenseType LicenseType,
    string? LicenseNumber,
    DateTime? LicenseExpiryDate
);

public record UpdateDriverDto(
    string Name,
    string? PhoneNumber,
    LicenseType LicenseType,
    string? LicenseNumber,
    DateTime? LicenseExpiryDate,
    bool IsAvailable,
    string Timezone,
    int? WorkdayStartTime,
    int? WorkdayEndTime
);
