namespace Hiberus.Industria.Vektor.Domain.Entities;

public class ActiveVehicleRoute
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid VehicleId { get; set; }
    public string RoutePayload { get; set; } = string.Empty; // JSON — geometría, waypoints
    public string AssociatedOrderIds { get; set; } = string.Empty; // JSON — array de Order IDs
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Tenant Tenant { get; set; } = null!;
}
