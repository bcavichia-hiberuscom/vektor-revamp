using ErrorOr;
using Hiberus.Industria.Vektor.Domain.Common;
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
        var tenantIdResult = Guard.NotEmpty(
            tenantId,
            "ActiveVehicleRoute.TenantId",
            "TenantId cannot be empty"
        );
        if (tenantIdResult.IsError)
            return tenantIdResult.Errors;

        var vehicleIdResult = Guard.NotEmpty(
            vehicleId,
            "ActiveVehicleRoute.VehicleId",
            "VehicleId cannot be empty"
        );
        if (vehicleIdResult.IsError)
            return vehicleIdResult.Errors;

        var routePayloadResult = Guard.NotNullOrWhiteSpace(
            routePayload,
            "ActiveVehicleRoute.RoutePayload",
            "RoutePayload is required"
        );
        if (routePayloadResult.IsError)
            return routePayloadResult.Errors;

        var createdByResult = Guard.NotNullOrWhiteSpace(
            createdBy,
            "ActiveVehicleRoute.CreatedBy",
            "CreatedBy is required"
        );
        if (createdByResult.IsError)
            return createdByResult.Errors;

        return new ActiveVehicleRoute
        {
            Id = Guid.NewGuid(),
            TenantId = tenantIdResult.Value,
            VehicleId = vehicleIdResult.Value,
            RoutePayload = routePayloadResult.Value,
            AssociatedOrderIds = associatedOrderIds,
            StartedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdByResult.Value,
        };
    }

    public ErrorOr<ActiveVehicleRoute> UpdateRoute(
        string routePayload,
        string associatedOrderIds,
        string updatedBy
    )
    {
        var routePayloadResult = Guard.NotNullOrWhiteSpace(
            routePayload,
            "ActiveVehicleRoute.RoutePayload",
            "RoutePayload is required"
        );
        if (routePayloadResult.IsError)
            return routePayloadResult.Errors;

        var updatedByResult = Guard.NotNullOrWhiteSpace(
            updatedBy,
            "ActiveVehicleRoute.UpdatedBy",
            "UpdatedBy is required"
        );
        if (updatedByResult.IsError)
            return updatedByResult.Errors;

        RoutePayload = routePayloadResult.Value;
        AssociatedOrderIds = associatedOrderIds;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedByResult.Value;

        return this;
    }

    // Devuelve los datos necesarios para construir el RouteHistory en el service
    public ErrorOr<RouteHistory.RouteHistory> Complete(string completedBy)
    {
        var completedByResult = Guard.NotNullOrWhiteSpace(
            completedBy,
            "ActiveVehicleRoute.CompletedBy",
            "CompletedBy is required"
        );
        if (completedByResult.IsError)
            return completedByResult.Errors;

        return RouteHistory.RouteHistory.Create(
            tenantId: TenantId,
            vehicleId: VehicleId,
            routePayload: RoutePayload,
            associatedOrderIds: AssociatedOrderIds,
            startedAt: StartedAt,
            createdBy: completedByResult.Value
        );
    }
}
