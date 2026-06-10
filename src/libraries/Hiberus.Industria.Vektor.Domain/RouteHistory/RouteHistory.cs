using ErrorOr;
using Hiberus.Industria.Vektor.Domain.Common;
using Hiberus.Industria.Vektor.Domain.Common.Interfaces;

namespace Hiberus.Industria.Vektor.Domain.RouteHistory;

public sealed class RouteHistory : IAuditable
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid VehicleId { get; private set; }
    public string RoutePayload { get; private set; } = string.Empty;
    public string AssociatedOrderIds { get; private set; } = string.Empty;
    public DateTime StartedAt { get; private set; }
    public DateTime FinishedAt { get; private set; }

    // IAuditable
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; } // siempre null: es inmutable
    public string CreatedBy { get; private set; } = string.Empty;
    public string? UpdatedBy { get; private set; } // siempre null: es inmutable

    // Navigation properties
    public Tenant.Tenant Tenant { get; private set; } = null!;
    public Vehicle.Vehicle Vehicle { get; private set; } = null!;

    private RouteHistory() { }

    // Solo se crea desde ActiveVehicleRoute.Complete() — no hay Create público por API
    internal static ErrorOr<RouteHistory> Create(
        Guid tenantId,
        Guid vehicleId,
        string routePayload,
        string associatedOrderIds,
        DateTime startedAt,
        string createdBy
    )
    {
        var tenantIdResult = Guard.NotEmpty(tenantId, "RouteHistory.TenantId", "TenantId cannot be empty");
        if (tenantIdResult.IsError)
            return tenantIdResult.Errors;

        var vehicleIdResult = Guard.NotEmpty(vehicleId, "RouteHistory.VehicleId", "VehicleId cannot be empty");
        if (vehicleIdResult.IsError)
            return vehicleIdResult.Errors;

        var routePayloadResult = Guard.NotNullOrWhiteSpace(routePayload, "RouteHistory.RoutePayload", "RoutePayload is required");
        if (routePayloadResult.IsError)
            return routePayloadResult.Errors;

        var now = DateTime.UtcNow;

        return new RouteHistory
        {
            Id = Guid.NewGuid(),
            TenantId = tenantIdResult.Value,
            VehicleId = vehicleIdResult.Value,
            RoutePayload = routePayloadResult.Value,
            AssociatedOrderIds = associatedOrderIds,
            StartedAt = startedAt,
            FinishedAt = now,
            CreatedAt = now,
            CreatedBy = createdBy,
        };
    }
}
