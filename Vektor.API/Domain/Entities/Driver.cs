namespace Vektor.API.Domain.Entities;

public class Driver
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public LicenseType LicenseType { get; set; }
    public string? LicenseNumber { get; set; }
    public DateTime? LicenseExpiryDate { get; set; }
    public bool IsAvailable { get; set; } = true;
    public string? ImageUrl { get; set; }

    // Workday
    public int? WorkdayStartTime { get; set; } // segundos desde medianoche
    public int? WorkdayEndTime { get; set; } // segundos desde medianoche
    public string Timezone { get; set; } = "UTC";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Tenant Tenant { get; set; } = null!;
    public ICollection<DriverVehicleAssignment> VehicleAssignments { get; set; } = [];
}

public enum LicenseType
{
    B, // Furgoneta
    C1, // Camión ligero hasta 7.500kg
    C, // Camión rígido
    CE, // Camión con remolque
}
