using Hiberus.Industria.Vektor.Domain.User;

namespace Hiberus.Industria.Vektor.Application.DTOs.User;

/// <summary>
/// Minimal user information without collections to prevent circular references.
/// Used for nested references in other DTOs.
/// </summary>
public record UserSummaryDto(Guid Id, string Email, string Name, UserRole Role);
