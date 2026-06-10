using ErrorOr;
using Hiberus.Industria.Vektor.Domain.Common;
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

    public static ErrorOr<Driver> Create(
        Guid tenantId,
        string name,
        LicenseType licenseType,
        string createdBy,
        string? phoneNumber = null,
        string? licenseNumber = null,
        DateTime? licenseExpiryDate = null
    )
    {
        var tenantIdResult = Guard.NotEmpty(
            tenantId,
            "Driver.TenantId",
            "TenantId cannot be empty"
        );
        if (tenantIdResult.IsError)
            return tenantIdResult.Errors;

        var nameResult = Guard.NotNullOrWhiteSpace(name, "Driver.Name", "Name is required");
        if (nameResult.IsError)
            return nameResult.Errors;

        return new Driver
        {
            Id = Guid.NewGuid(),
            TenantId = tenantIdResult.Value,
            Name = nameResult.Value,
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

    public ErrorOr<Driver> Update(
        string name,
        string? phoneNumber,
        LicenseType licenseType,
        string? licenseNumber,
        DateTime? licenseExpiryDate,
        bool isAvailable,
        string timezone,
        int? workdayStartTime,
        int? workdayEndTime,
        string updatedBy
    )
    {
        var nameResult = Guard.NotNullOrWhiteSpace(name, "Driver.Name", "Name is required");
        if (nameResult.IsError)
            return nameResult.Errors;

        var timezoneResult = Guard.NotNullOrWhiteSpace(
            timezone,
            "Driver.Timezone",
            "Timezone is required"
        );
        if (timezoneResult.IsError)
            return timezoneResult.Errors;

        Name = nameResult.Value;
        PhoneNumber = phoneNumber?.Trim();
        LicenseType = licenseType;
        LicenseNumber = licenseNumber?.Trim();
        LicenseExpiryDate = licenseExpiryDate;
        IsAvailable = isAvailable;
        Timezone = timezoneResult.Value;
        WorkdayStartTime = workdayStartTime;
        WorkdayEndTime = workdayEndTime;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;

        return this;
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
