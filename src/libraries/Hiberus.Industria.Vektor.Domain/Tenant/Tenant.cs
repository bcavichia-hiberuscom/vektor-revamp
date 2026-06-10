using ErrorOr;
using Hiberus.Industria.Vektor.Domain.Common;
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
        var nameResult = Guard.NotNullOrWhiteSpace(name, "Tenant.Name", "Name is required");
        if (nameResult.IsError)
            return nameResult.Errors;

        var slugResult = Guard.NotNullOrWhiteSpace(slug, "Tenant.Slug", "Slug is required");
        if (slugResult.IsError)
            return slugResult.Errors;

        var createdByResult = Guard.NotNullOrWhiteSpace(
            createdBy,
            "Tenant.CreatedBy",
            "CreatedBy is required"
        );
        if (createdByResult.IsError)
            return createdByResult.Errors;

        return new Tenant
        {
            Id = Guid.NewGuid(),
            Name = nameResult.Value,
            Slug = slugResult.Value.ToLowerInvariant(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdByResult.Value,
        };
    }

    // Only Name is editable; Slug is immutable by design
    public ErrorOr<Tenant> Update(string name, string updatedBy)
    {
        if (IsDeleted)
            return Error.Conflict("Tenant.Deleted", "Cannot update a deleted tenant");

        var nameResult = Guard.NotNullOrWhiteSpace(name, "Tenant.Name", "Name is required");
        if (nameResult.IsError)
            return nameResult.Errors;

        var updatedByResult = Guard.NotNullOrWhiteSpace(
            updatedBy,
            "Tenant.UpdatedBy",
            "UpdatedBy is required"
        );
        if (updatedByResult.IsError)
            return updatedByResult.Errors;

        Name = nameResult.Value;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedByResult.Value;

        return this;
    }

    public ErrorOr<Tenant> Delete(string deletedBy)
    {
        if (IsDeleted)
            return Error.Conflict("Tenant.Deleted", "Tenant is already deleted");

        var deletedByResult = Guard.NotNullOrWhiteSpace(
            deletedBy,
            "Tenant.DeletedBy",
            "DeletedBy is required"
        );
        if (deletedByResult.IsError)
            return deletedByResult.Errors;

        IsActive = false;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedByResult.Value;

        return this;
    }
}
