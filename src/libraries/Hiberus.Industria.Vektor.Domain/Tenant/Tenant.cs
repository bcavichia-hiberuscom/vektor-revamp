using ErrorOr;
using Hiberus.Industria.Vektor.Domain.Common.Interfaces;

namespace Hiberus.Industria.Vektor.Domain.Tenant;

public sealed class Tenant : IAuditable
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public string CreatedBy { get; private set; } = string.Empty;
    public string? UpdatedBy { get; private set; }

    // Navigation properties
    public IReadOnlyCollection<User.User> Users { get; private set; } = [];
    public IReadOnlyCollection<Vehicle.Vehicle> Vehicles { get; private set; } = [];
    public IReadOnlyCollection<Driver.Driver> Drivers { get; private set; } = [];
    public IReadOnlyCollection<Order.Order> Orders { get; private set; } = [];

    private Tenant() { }

    public static ErrorOr<Tenant> Create(string name, string slug, string createdBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Error.Validation("Tenant.Name", "Name is required");
        if (string.IsNullOrWhiteSpace(slug))
            return Error.Validation("Tenant.Slug", "Slug is required");

        return new Tenant
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Slug = slug.Trim().ToLowerInvariant(),
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy,
        };
    }
}
