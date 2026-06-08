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

    private ActiveVehicleRoute() { }

    public static ActiveVehicleRoute Create(
        Guid tenantId,
        Guid vehicleId,
        string routePayload,
        string associatedOrderIds,
        string createdBy
    )
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId cannot be empty");
        if (vehicleId == Guid.Empty)
            throw new ArgumentException("VehicleId cannot be empty");
        if (string.IsNullOrWhiteSpace(routePayload))
            throw new ArgumentException("RoutePayload is required");

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

    public void UpdateRoute(string routePayload, string associatedOrderIds, string updatedBy)
    {
        RoutePayload = routePayload;
        AssociatedOrderIds = associatedOrderIds;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }
}
