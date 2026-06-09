namespace Hiberus.Industria.Vektor.Application.DTOs;

public record OrderDto(
    Guid Id,
    Guid TenantId,
    string Label,
    string? Description,
    double Latitude,
    double Longitude,
    string? CustomerName,
    string? CustomerPhone,
    string? ExternalOrderId,
    string Status
);

public record CreateOrderDto(
    string Label,
    double Latitude,
    double Longitude,
    string? ExternalOrderId,
    string? Description,
    string? CustomerName,
    string? CustomerPhone
);

public record UpdateOrderDto(
    string Label,
    double Latitude,
    double Longitude,
    string? ExternalOrderId,
    string? Description,
    string? CustomerName,
    string? CustomerPhone
);
