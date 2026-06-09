namespace Hiberus.Industria.Vektor.Application.DTOs;

public record TenantDto(
    Guid Id,
    string Name,
    string Slug,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record CreateTenantDto(string Name, string Slug);

public record UpdateTenantDto(string Name);
