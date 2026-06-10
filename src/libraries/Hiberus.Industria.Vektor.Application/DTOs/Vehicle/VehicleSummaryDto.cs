using Hiberus.Industria.Vektor.Domain.Vehicle;

namespace Hiberus.Industria.Vektor.Application.DTOs.Vehicle;

/// <summary>
/// Minimal vehicle information without collections to prevent circular references.
/// Used for nested references in other DTOs.
/// </summary>
public record VehicleSummaryDto(
    Guid Id,
    string Label,
    string LicensePlate,
    string Brand,
    string Model,
    int Year,
    VehicleType Type,
    string Status
);
