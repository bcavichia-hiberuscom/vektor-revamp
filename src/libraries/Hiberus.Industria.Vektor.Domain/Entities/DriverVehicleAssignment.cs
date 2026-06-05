namespace Hiberus.Industria.Vektor.Domain.Entities;

public class DriverVehicleAssignment
{
    public Guid Id { get; set; }
    public Guid DriverId { get; set; }
    public Guid VehicleId { get; set; }
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UnassignedAt { get; set; }

    // Navigation properties
    public Driver Driver { get; set; } = null!;
    public Vehicle Vehicle { get; set; } = null!;
}
