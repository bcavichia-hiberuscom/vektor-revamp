namespace Vektor.API.Domain.Entities;

public class Tenant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<User> Users { get; set; } = [];
    public ICollection<Vehicle> Vehicles { get; set; } = [];
    public ICollection<Driver> Drivers { get; set; } = [];
    public ICollection<Order> Orders { get; set; } = [];
}
