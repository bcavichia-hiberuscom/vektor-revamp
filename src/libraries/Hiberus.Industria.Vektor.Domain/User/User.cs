using ErrorOr;
using Hiberus.Industria.Vektor.Domain.Common.Interfaces;

namespace Hiberus.Industria.Vektor.Domain.User;

public sealed class User : IAuditable
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string? Name { get; private set; }
    public UserRole Role { get; private set; } = UserRole.Driver;
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
    public Tenant.Tenant Tenant { get; private set; } = null!;

    private User() { }

    public static ErrorOr<User> Create(
        Guid tenantId,
        string email,
        string? name,
        UserRole role,
        string createdBy
    )
    {
        if (tenantId == Guid.Empty)
            return Error.Validation("User.TenantId", "TenantId cannot be empty");
        if (string.IsNullOrWhiteSpace(email))
            return Error.Validation("User.Email", "Email is required");
        if (string.IsNullOrWhiteSpace(createdBy))
            return Error.Validation("User.CreatedBy", "CreatedBy is required");

        return new User
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Email = email.Trim().ToLowerInvariant(),
            Name = name?.Trim(),
            Role = role,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy,
        };
    }

    public ErrorOr<User> Update(string? name, UserRole role, bool isActive, string updatedBy)
    {
        if (IsDeleted)
            return Error.Conflict("User.Deleted", "Cannot update a deleted user");
        if (string.IsNullOrWhiteSpace(updatedBy))
            return Error.Validation("User.UpdatedBy", "UpdatedBy is required");

        Name = name?.Trim();
        Role = role;
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;

        return this;
    }

    public ErrorOr<User> Delete(string deletedBy)
    {
        if (IsDeleted)
            return Error.Conflict("User.Deleted", "User is already deleted");
        if (string.IsNullOrWhiteSpace(deletedBy))
            return Error.Validation("User.DeletedBy", "DeletedBy is required");

        IsActive = false;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;

        return this;
    }
}
