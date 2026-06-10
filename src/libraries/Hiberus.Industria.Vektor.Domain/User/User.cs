using ErrorOr;
using Hiberus.Industria.Vektor.Domain.Common;
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
        var tenantIdResult = Guard.NotEmpty(tenantId, "User.TenantId", "TenantId cannot be empty");
        if (tenantIdResult.IsError)
            return tenantIdResult.Errors;

        var emailResult = Guard.NotNullOrWhiteSpace(email, "User.Email", "Email is required");
        if (emailResult.IsError)
            return emailResult.Errors;

        var createdByResult = Guard.NotNullOrWhiteSpace(
            createdBy,
            "User.CreatedBy",
            "CreatedBy is required"
        );
        if (createdByResult.IsError)
            return createdByResult.Errors;

        return new User
        {
            Id = Guid.NewGuid(),
            TenantId = tenantIdResult.Value,
            Email = emailResult.Value.ToLowerInvariant(),
            Name = name?.Trim(),
            Role = role,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdByResult.Value,
        };
    }

    public ErrorOr<User> Update(string? name, UserRole role, bool isActive, string updatedBy)
    {
        if (IsDeleted)
            return Error.Conflict("User.Deleted", "Cannot update a deleted user");

        var updatedByResult = Guard.NotNullOrWhiteSpace(
            updatedBy,
            "User.UpdatedBy",
            "UpdatedBy is required"
        );
        if (updatedByResult.IsError)
            return updatedByResult.Errors;

        Name = name?.Trim();
        Role = role;
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedByResult.Value;

        return this;
    }

    public ErrorOr<User> Delete(string deletedBy)
    {
        if (IsDeleted)
            return Error.Conflict("User.Deleted", "User is already deleted");

        var deletedByResult = Guard.NotNullOrWhiteSpace(
            deletedBy,
            "User.DeletedBy",
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
