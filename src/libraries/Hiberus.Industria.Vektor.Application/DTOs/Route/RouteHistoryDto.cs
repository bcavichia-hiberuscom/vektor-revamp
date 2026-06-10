using Hiberus.Industria.Vektor.Application.DTOs.Tenant;
using Hiberus.Industria.Vektor.Application.DTOs.Vehicle;

namespace Hiberus.Industria.Vektor.Application.DTOs.Route;

/// <summary>
/// Route history information with nested vehicle and tenant.
/// Includes 1-level nesting to avoid circular references.
/// </summary>
public record RouteHistoryDto(
    Guid Id,
    Guid TenantId,
    Guid VehicleId,
    string RoutePayload,
    string AssociatedOrderIds,
    DateTime StartedAt,
    DateTime FinishedAt,
    TimeSpan Duration,
    VehicleSummaryDto Vehicle,
    TenantSummaryDto Tenant
);
