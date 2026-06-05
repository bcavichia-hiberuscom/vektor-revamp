using Vektor.API.Domain.Entities;
using Vektor.API.Domain.Enums;

namespace Vektor.API.Application.DTOs;

public record VehicleDto(
    Guid Id,
    string Label,
    string? LicensePlate,
    string? Brand,
    string? Model,
    int? Year,
    VehicleType Type,
    VehicleStatus Status
);

public record VehicleTelemetryDto(
    Guid VehicleId,
    float? FuelLevel,
    float? BatteryLevel,
    float? EngineTemp,
    float? Rpm,
    float? Speed,
    float? Odometer,
    double? Latitude,
    double? Longitude,
    DateTime? LastGpsUpdate
);

public record CreateVehicleDto(
    string Label,
    string? LicensePlate,
    string? Brand,
    string? Model,
    int? Year,
    VehicleType Type
);

public record UpdateVehicleDto(
    string Label,
    string? LicensePlate,
    string? Brand,
    string? Model,
    int? Year,
    string Status
);
