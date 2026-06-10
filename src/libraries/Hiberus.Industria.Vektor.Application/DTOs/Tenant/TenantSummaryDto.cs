namespace Hiberus.Industria.Vektor.Application.DTOs.Tenant;

/// <summary>
/// Minimal tenant information without collections to prevent circular references.
/// Used for nested references in other DTOs.
/// </summary>
public record TenantSummaryDto(Guid Id, string Name, string Slug);
