using ErrorOr;
using Hiberus.Industria.Vektor.Domain.Common.Interfaces;

namespace Hiberus.Industria.Vektor.Domain.Tenant;

public sealed class Tenant : IAuditable
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty; // immutable after creation
    public bool IsActive { get; private set; } = true;

    // Auditable
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public string CreatedBy { get; private set; } = string.Empty;
    public string? UpdatedBy { get; private set; }

    // Soft delete
    public DateTime? DeletedAt { get; private set; }
    public string? DeletedBy { get; private set; }
    public bool IsDeleted => DeletedAt.HasValue;

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
        if (string.IsNullOrWhiteSpace(createdBy))
            return Error.Validation("Tenant.CreatedBy", "CreatedBy is required");

        return new Tenant
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Slug = slug.Trim().ToLowerInvariant(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy,
        };
    }

    // Only Name is editable; Slug is immutable by design
    public ErrorOr<Tenant> Update(string name, string updatedBy)
    {
        if (IsDeleted)
            return Error.Conflict("Tenant.Deleted", "Cannot update a deleted tenant");
        if (string.IsNullOrWhiteSpace(name))
            return Error.Validation("Tenant.Name", "Name is required");
        if (string.IsNullOrWhiteSpace(updatedBy))
            return Error.Validation("Tenant.UpdatedBy", "UpdatedBy is required");

        Name = name.Trim();
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;

        return this;
    }

    public ErrorOr<Tenant> Delete(string deletedBy)
    {
        if (IsDeleted)
            return Error.Conflict("Tenant.Deleted", "Tenant is already deleted");
        if (string.IsNullOrWhiteSpace(deletedBy))
            return Error.Validation("Tenant.DeletedBy", "DeletedBy is required");

        IsActive = false;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;

        return this;
    }
}
