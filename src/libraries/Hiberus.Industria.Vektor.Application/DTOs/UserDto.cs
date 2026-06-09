using Hiberus.Industria.Vektor.Domain.User;

namespace Hiberus.Industria.Vektor.Application.DTOs;

public record UserDto(
    Guid Id,
    Guid TenantId,
    string Email,
    string? Name,
    UserRole Role,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record CreateUserDto(Guid TenantId, string Email, string? Name, UserRole Role);

public record UpdateUserDto(string? Name, UserRole Role, bool IsActive);
