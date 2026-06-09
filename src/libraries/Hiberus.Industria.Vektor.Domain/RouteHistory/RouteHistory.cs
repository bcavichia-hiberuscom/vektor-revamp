using ErrorOr;
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
        if (tenantId == Guid.Empty)
            return Error.Validation("RouteHistory.TenantId", "TenantId cannot be empty");
        if (vehicleId == Guid.Empty)
            return Error.Validation("RouteHistory.VehicleId", "VehicleId cannot be empty");
        if (string.IsNullOrWhiteSpace(routePayload))
            return Error.Validation("RouteHistory.RoutePayload", "RoutePayload is required");

        var now = DateTime.UtcNow;

        return new RouteHistory
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            VehicleId = vehicleId,
            RoutePayload = routePayload,
            AssociatedOrderIds = associatedOrderIds,
            StartedAt = startedAt,
            FinishedAt = now,
            CreatedAt = now,
            CreatedBy = createdBy,
        };
    }
}
