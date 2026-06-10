using Hiberus.Industria.Vektor.Application.DTOs.Tenant;
using Hiberus.Industria.Vektor.Application.DTOs.Vehicle;

namespace Hiberus.Industria.Vektor.Application.DTOs.Route;

/// <summary>
/// Active vehicle route information with nested vehicle and tenant.
/// Includes 1-level nesting to avoid circular references.
/// </summary>
public record ActiveVehicleRouteDto(
    Guid Id,
    Guid TenantId,
    Guid VehicleId,
    string RoutePayload,
    string AssociatedOrderIds,
    DateTime StartedAt,
    DateTime CreatedAt,
    VehicleSummaryDto Vehicle,
    TenantSummaryDto Tenant
);

public record CreateActiveVehicleRouteDto(
    Guid VehicleId,
    string RoutePayload,
    string AssociatedOrderIds
);

public record UpdateActiveVehicleRouteDto(string RoutePayload, string AssociatedOrderIds);
