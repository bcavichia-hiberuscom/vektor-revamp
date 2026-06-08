using Hiberus.Industria.Vektor.Domain.Common.Interfaces;

namespace Hiberus.Industria.Vektor.Domain.User;

public sealed class User : IAuditable
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string? Name { get; private set; }
    public string Role { get; private set; } = "user";
    public bool IsActive { get; private set; } = true;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public string CreatedBy { get; private set; } = string.Empty;
    public string? UpdatedBy { get; private set; }

    // Navigation properties
    public Tenant.Tenant Tenant { get; private set; } = null!;

    private User() { }

    public static User Create(
        Guid tenantId,
        string email,
        string? name,
        string role,
        string createdBy
    )
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId cannot be empty");
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required");

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
}
