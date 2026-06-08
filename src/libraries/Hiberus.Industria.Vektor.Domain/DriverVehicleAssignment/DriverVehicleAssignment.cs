using Hiberus.Industria.Vektor.Domain.Common.Interfaces;

namespace Hiberus.Industria.Vektor.Domain.DriverVehicleAssignment;

public sealed class DriverVehicleAssignment : IAuditable
{
    public Guid Id { get; private set; }
    public Guid DriverId { get; private set; }
    public Guid VehicleId { get; private set; }
    public DateTime AssignedAt { get; private set; }
    public DateTime? UnassignedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public string CreatedBy { get; private set; } = string.Empty;
    public string? UpdatedBy { get; private set; }

    // Navigation properties
    public Driver.Driver Driver { get; private set; } = null!;
    public Vehicle.Vehicle Vehicle { get; private set; } = null!;

    private DriverVehicleAssignment() { }

    public static DriverVehicleAssignment Create(Guid driverId, Guid vehicleId, string createdBy)
    {
        if (driverId == Guid.Empty)
            throw new ArgumentException("DriverId cannot be empty");
        if (vehicleId == Guid.Empty)
            throw new ArgumentException("VehicleId cannot be empty");

        return new DriverVehicleAssignment
        {
            Id = Guid.NewGuid(),
            DriverId = driverId,
            VehicleId = vehicleId,
            AssignedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy,
        };
    }

    public void Unassign(string updatedBy)
    {
        UnassignedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }
}
