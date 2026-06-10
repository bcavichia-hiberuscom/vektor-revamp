using Hiberus.Industria.Vektor.Application.DTOs.Tenant;
using Hiberus.Industria.Vektor.Domain.User;

namespace Hiberus.Industria.Vektor.Application.DTOs.User;

/// <summary>
/// Complete user information with nested tenant.
/// Includes 1-level nesting to avoid circular references.
/// </summary>
public record UserDto(
    Guid Id,
    Guid TenantId,
    string Email,
    string? Name,
    UserRole Role,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    TenantSummaryDto Tenant
);

public record CreateUserDto(Guid TenantId, string Email, string? Name, UserRole Role);

public record UpdateUserDto(string? Name, UserRole Role, bool IsActive);
