// Vektor.API/Domain/Entities/Vehicle.cs
namespace Vektor.API.Domain.Entities;

public class Vehicle
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Label { get; set; } = string.Empty;
    public string? LicensePlate { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public int? Year { get; set; }
    public VehicleType Type { get; set; }
    public string Status { get; set; } = "active";

    // Telemetry
    public string? DeviceImei { get; set; }
    public float? FuelLevel { get; set; } // %
    public float? BatteryLevel { get; set; } // %
    public float? EngineTemp { get; set; } // °C
    public float? Rpm { get; set; }
    public float? Speed { get; set; } // km/h
    public float? Odometer { get; set; } // km
    public string? DtcCodes { get; set; } // JSON array of active codes
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public DateTime? LastGpsUpdate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Tenant Tenant { get; set; } = null!;
    public ICollection<DriverVehicleAssignment> Assignments { get; set; } = [];
    public ICollection<OrderAssignment> OrderAssignments { get; set; } = [];
}

public enum VehicleType
{
    Van,
    Truck,
    TruckWithTrailer,
}
