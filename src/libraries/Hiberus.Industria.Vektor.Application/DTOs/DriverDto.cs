using Hiberus.Industria.Vektor.Domain.Driver;

namespace Hiberus.Industria.Vektor.Application.DTOs;

public record DriverDto(
    Guid Id,
    Guid TenantId,
    string Name,
    string? PhoneNumber,
    LicenseType LicenseType,
    string? LicenseNumber,
    DateTime? LicenseExpiryDate,
    bool IsAvailable,
    string? ImageUrl,
    int? WorkdayStartTime,
    int? WorkdayEndTime,
    string Timezone
);

public record CreateDriverDto(
    string Name,
    string? PhoneNumber,
    LicenseType LicenseType,
    string? LicenseNumber,
    DateTime? LicenseExpiryDate
);

public record UpdateDriverDto(
    string Name,
    string? PhoneNumber,
    LicenseType LicenseType,
    string? LicenseNumber,
    DateTime? LicenseExpiryDate,
    bool IsAvailable,
    string Timezone,
    int? WorkdayStartTime,
    int? WorkdayEndTime
);
