using ErrorOr;
using Hiberus.Industria.Vektor.Domain.Common;
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

    public Tenant.Tenant Tenant { get; private set; } = null!;
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
        var tenantIdResult = Guard.NotEmpty(
            tenantId,
            "DriverVehicleAssignment.TenantId",
            "TenantId cannot be empty"
        );
        if (tenantIdResult.IsError)
            return tenantIdResult.Errors;

        var driverIdResult = Guard.NotEmpty(
            driverId,
            "DriverVehicleAssignment.DriverId",
            "DriverId cannot be empty"
        );
        if (driverIdResult.IsError)
            return driverIdResult.Errors;

        var vehicleIdResult = Guard.NotEmpty(
            vehicleId,
            "DriverVehicleAssignment.VehicleId",
            "VehicleId cannot be empty"
        );
        if (vehicleIdResult.IsError)
            return vehicleIdResult.Errors;

        return new DriverVehicleAssignment
        {
            Id = Guid.NewGuid(),
            TenantId = tenantIdResult.Value,
            DriverId = driverIdResult.Value,
            VehicleId = vehicleIdResult.Value,
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
