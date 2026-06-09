using ErrorOr;
using Hiberus.Industria.Vektor.Domain.Common.Interfaces;

namespace Hiberus.Industria.Vektor.Domain.DriverVehicleAssignment;

public sealed class DriverVehicleAssignment : IAuditable
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }

    public Guid DriverId { get; private set; }
    public Guid VehicleId { get; private set; }

    public DateTime AssignedAt { get; private set; }
    public DateTime? UnassignedAt { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public string CreatedBy { get; private set; } = string.Empty;
    public string? UpdatedBy { get; private set; }

    public Driver.Driver Driver { get; private set; } = null!;
    public Vehicle.Vehicle Vehicle { get; private set; } = null!;

    private DriverVehicleAssignment() { }

    public static ErrorOr<DriverVehicleAssignment> Create(
        Guid tenantId,
        Guid driverId,
        Guid vehicleId,
        string createdBy
    )
    {
        if (tenantId == Guid.Empty)
            return Error.Validation("DriverVehicleAssignment.TenantId", "TenantId cannot be empty");

        if (driverId == Guid.Empty)
            return Error.Validation("DriverVehicleAssignment.DriverId", "DriverId cannot be empty");

        if (vehicleId == Guid.Empty)
            return Error.Validation(
                "DriverVehicleAssignment.VehicleId",
                "VehicleId cannot be empty"
            );

        return new DriverVehicleAssignment
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            DriverId = driverId,
            VehicleId = vehicleId,
            AssignedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy,
        };
    }

    public ErrorOr<DriverVehicleAssignment> Unassign(string updatedBy)
    {
        if (UnassignedAt is not null)
            return Error.Conflict("DriverVehicleAssignment.UnassignedAt", "Already unassigned");

        UnassignedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;

        return this;
    }
}
