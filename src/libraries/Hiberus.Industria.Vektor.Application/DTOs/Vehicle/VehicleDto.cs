using Hiberus.Industria.Vektor.Application.DTOs.DriverVehicleAssignment;
using Hiberus.Industria.Vektor.Application.DTOs.OrderAssignment;
using Hiberus.Industria.Vektor.Application.DTOs.Tenant;
using Hiberus.Industria.Vektor.Domain.Vehicle;

namespace Hiberus.Industria.Vektor.Application.DTOs.Vehicle;

/// <summary>
/// Complete vehicle information with nested tenant and assignment collections.
/// Includes 1-level nesting to avoid circular references.
/// </summary>
public record VehicleDto(
    Guid Id,
    Guid TenantId,
    string Label,
    string? LicensePlate,
    string? Brand,
    string? Model,
    int? Year,
    VehicleType Type,
    string Status,
    TenantSummaryDto Tenant,
    IReadOnlyCollection<DriverVehicleAssignmentDto> DriverAssignments,
    IReadOnlyCollection<OrderAssignmentDto> OrderAssignments
);

public record VehicleTelemetryDto(
    Guid VehicleId,
    float? FuelLevel,
    float? BatteryLevel,
    float? EngineTemp,
    float? Rpm,
    float? Speed,
    float? Odometer,
    double? Latitude,
    double? Longitude,
    DateTime? LastGpsUpdate
);

public record CreateVehicleDto(
    string Label,
    string? LicensePlate,
    string? Brand,
    string? Model,
    int? Year,
    VehicleType Type
);

public record UpdateVehicleDto(
    string Label,
    string? LicensePlate,
    string? Brand,
    string? Model,
    int? Year
);
