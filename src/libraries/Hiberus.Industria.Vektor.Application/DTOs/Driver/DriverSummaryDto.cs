using Hiberus.Industria.Vektor.Domain.Driver;

namespace Hiberus.Industria.Vektor.Application.DTOs.Driver;

/// <summary>
/// Minimal driver information without collections to prevent circular references.
/// Used for nested references in other DTOs.
/// </summary>
public record DriverSummaryDto(
    Guid Id,
    string Name,
    string? PhoneNumber,
    LicenseType LicenseType,
    bool IsAvailable
);
