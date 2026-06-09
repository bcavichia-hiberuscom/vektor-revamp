namespace Hiberus.Industria.Vektor.Application.DTOs;

//  ActiveVehicleRoute

public record ActiveVehicleRouteDto(
    Guid Id,
    Guid TenantId,
    Guid VehicleId,
    string RoutePayload,
    string AssociatedOrderIds,
    DateTime StartedAt,
    DateTime CreatedAt
);

public record CreateActiveVehicleRouteDto(
    Guid VehicleId,
    string RoutePayload,
    string AssociatedOrderIds
);

public record UpdateActiveVehicleRouteDto(string RoutePayload, string AssociatedOrderIds);

// RouteHistory

public record RouteHistoryDto(
    Guid Id,
    Guid TenantId,
    Guid VehicleId,
    string RoutePayload,
    string AssociatedOrderIds,
    DateTime StartedAt,
    DateTime FinishedAt,
    TimeSpan Duration
);
