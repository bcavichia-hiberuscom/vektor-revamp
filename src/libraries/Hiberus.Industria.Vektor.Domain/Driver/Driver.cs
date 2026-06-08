using Hiberus.Industria.Vektor.Domain.Common.Interfaces;

namespace Hiberus.Industria.Vektor.Domain.Driver;

public sealed class Driver : IAuditable
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? PhoneNumber { get; private set; }
    public LicenseType LicenseType { get; private set; }
    public string? LicenseNumber { get; private set; }
    public DateTime? LicenseExpiryDate { get; private set; }
    public bool IsAvailable { get; private set; } = true;
    public string? ImageUrl { get; private set; }
    public int? WorkdayStartTime { get; private set; }
    public int? WorkdayEndTime { get; private set; }
    public string Timezone { get; private set; } = "UTC";
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public string CreatedBy { get; private set; } = string.Empty;
    public string? UpdatedBy { get; private set; }

    // Navigation properties
    public Tenant.Tenant Tenant { get; private set; } = null!;
    public IReadOnlyCollection<DriverVehicleAssignment.DriverVehicleAssignment> VehicleAssignments
    {
        get;
        private set;
    } = [];

    private Driver() { }

    public static Driver Create(
        Guid tenantId,
        string name,
        LicenseType licenseType,
        string createdBy,
        string? phoneNumber = null,
        string? licenseNumber = null,
        DateTime? licenseExpiryDate = null
    )
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId cannot be empty");
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required");

        return new Driver
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Name = name.Trim(),
            LicenseType = licenseType,
            PhoneNumber = phoneNumber?.Trim(),
            LicenseNumber = licenseNumber?.Trim(),
            LicenseExpiryDate = licenseExpiryDate,
            IsAvailable = true,
            Timezone = "UTC",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy,
        };
    }

    public bool CanDrive(Vehicle.VehicleType vehicleType) =>
        vehicleType switch
        {
            Vehicle.VehicleType.Van => LicenseType >= LicenseType.B,
            Vehicle.VehicleType.Truck => LicenseType >= LicenseType.C1,
            Vehicle.VehicleType.TruckWithTrailer => LicenseType == LicenseType.CE,
            _ => false,
        };
}
