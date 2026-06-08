using ErrorOr;
using Hiberus.Industria.Vektor.Domain.Common.Interfaces;

namespace Hiberus.Industria.Vektor.Domain.Vehicle;

public sealed class Vehicle : IAuditable
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Label { get; private set; } = string.Empty;
    public string? LicensePlate { get; private set; }
    public string? Brand { get; private set; }
    public string? Model { get; private set; }
    public int? Year { get; private set; }
    public VehicleType Type { get; private set; }
    public VehicleStatus Status { get; private set; }
    public string? DeviceImei { get; private set; }
    public float? FuelLevel { get; private set; }
    public float? BatteryLevel { get; private set; }
    public float? EngineTemp { get; private set; }
    public float? Rpm { get; private set; }
    public float? Speed { get; private set; }
    public float? Odometer { get; private set; }
    public string? DtcCodes { get; private set; }
    public double? Latitude { get; private set; }
    public double? Longitude { get; private set; }
    public DateTime? LastGpsUpdate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public string CreatedBy { get; private set; } = string.Empty;
    public string? UpdatedBy { get; private set; }

    // Navigation properties
    public Tenant.Tenant Tenant { get; private set; } = null!;
    public IReadOnlyCollection<DriverVehicleAssignment.DriverVehicleAssignment> Assignments
    {
        get;
        private set;
    } = [];
    public IReadOnlyCollection<OrderAssignment.OrderAssignment> OrderAssignments
    {
        get;
        private set;
    } = [];

    private Vehicle() { }

    public static ErrorOr<Vehicle> Create(
        Guid tenantId,
        string label,
        VehicleType type,
        string createdBy,
        string? licensePlate = null,
        string? brand = null,
        string? model = null,
        int? year = null
    )
    {
        if (tenantId == Guid.Empty)
            return Error.Validation("Vehicle.TenantId", "TenantId cannot be empty");
        if (string.IsNullOrWhiteSpace(label))
            return Error.Validation("Vehicle.Label", "Label is required");
        if (year is < 1900 or > 2100)
            return Error.Validation("Vehicle.Year", "Year is not valid");

        return new Vehicle
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Label = label.Trim(),
            LicensePlate = licensePlate?.Trim(),
            Brand = brand?.Trim(),
            Model = model?.Trim(),
            Year = year,
            Type = type,
            Status = VehicleStatus.Active,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy,
        };
    }

    public ErrorOr<Vehicle> Update(
        string label,
        string? licensePlate,
        string? brand,
        string? model,
        int? year,
        string updatedBy
    )
    {
        if (string.IsNullOrWhiteSpace(label))
            return Error.Validation("Vehicle.Label", "Label is required");
        if (year is < 1900 or > 2100)
            return Error.Validation("Vehicle.Year", "Year is not valid");

        Label = label.Trim();
        LicensePlate = licensePlate?.Trim();
        Brand = brand?.Trim();
        Model = model?.Trim();
        Year = year;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;

        return this;
    }

    public ErrorOr<Vehicle> Deactivate(string updatedBy)
    {
        if (Status == VehicleStatus.Inactive)
            return Error.Conflict("Vehicle.Status", "Vehicle is already inactive");

        Status = VehicleStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;

        return this;
    }

    public void UpdateTelemetry(
        float? fuelLevel,
        float? batteryLevel,
        float? engineTemp,
        float? rpm,
        float? speed,
        float? odometer,
        string? dtcCodes,
        double? latitude,
        double? longitude
    )
    {
        FuelLevel = fuelLevel;
        BatteryLevel = batteryLevel;
        EngineTemp = engineTemp;
        Rpm = rpm;
        Speed = speed;
        Odometer = odometer;
        DtcCodes = dtcCodes;
        Latitude = latitude;
        Longitude = longitude;
        LastGpsUpdate = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
