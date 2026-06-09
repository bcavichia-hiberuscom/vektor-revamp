using ErrorOr;
using Hiberus.Industria.Vektor.Domain.Common.Interfaces;

namespace Hiberus.Industria.Vektor.Domain.ActiveVehicleRoute;

public sealed class ActiveVehicleRoute : IAuditable
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid VehicleId { get; private set; }
    public string RoutePayload { get; private set; } = string.Empty;
    public string AssociatedOrderIds { get; private set; } = string.Empty;
    public DateTime StartedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public string CreatedBy { get; private set; } = string.Empty;
    public string? UpdatedBy { get; private set; }

    // Navigation properties
    public Tenant.Tenant Tenant { get; private set; } = null!;
    public Vehicle.Vehicle Vehicle { get; private set; } = null!;

    private ActiveVehicleRoute() { }

    public static ErrorOr<ActiveVehicleRoute> Create(
        Guid tenantId,
        Guid vehicleId,
        string routePayload,
        string associatedOrderIds,
        string createdBy
    )
    {
        if (tenantId == Guid.Empty)
            return Error.Validation("ActiveVehicleRoute.TenantId", "TenantId cannot be empty");
        if (vehicleId == Guid.Empty)
            return Error.Validation("ActiveVehicleRoute.VehicleId", "VehicleId cannot be empty");
        if (string.IsNullOrWhiteSpace(routePayload))
            return Error.Validation("ActiveVehicleRoute.RoutePayload", "RoutePayload is required");
        if (string.IsNullOrWhiteSpace(createdBy))
            return Error.Validation("ActiveVehicleRoute.CreatedBy", "CreatedBy is required");

        return new ActiveVehicleRoute
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            VehicleId = vehicleId,
            RoutePayload = routePayload,
            AssociatedOrderIds = associatedOrderIds,
            StartedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy,
        };
    }

    public ErrorOr<ActiveVehicleRoute> UpdateRoute(
        string routePayload,
        string associatedOrderIds,
        string updatedBy
    )
    {
        if (string.IsNullOrWhiteSpace(routePayload))
            return Error.Validation("ActiveVehicleRoute.RoutePayload", "RoutePayload is required");
        if (string.IsNullOrWhiteSpace(updatedBy))
            return Error.Validation("ActiveVehicleRoute.UpdatedBy", "UpdatedBy is required");

        RoutePayload = routePayload;
        AssociatedOrderIds = associatedOrderIds;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;

        return this;
    }

    // Devuelve los datos necesarios para construir el RouteHistory en el service
    public ErrorOr<RouteHistory.RouteHistory> Complete(string completedBy)
    {
        if (string.IsNullOrWhiteSpace(completedBy))
            return Error.Validation("ActiveVehicleRoute.CompletedBy", "CompletedBy is required");

        return RouteHistory.RouteHistory.Create(
            tenantId: TenantId,
            vehicleId: VehicleId,
            routePayload: RoutePayload,
            associatedOrderIds: AssociatedOrderIds,
            startedAt: StartedAt,
            createdBy: completedBy
        );
    }
}
