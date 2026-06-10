using Hiberus.Industria.Vektor.Application.DTOs.OrderAssignment;
using Hiberus.Industria.Vektor.Application.DTOs.Tenant;

namespace Hiberus.Industria.Vektor.Application.DTOs.Order;

/// <summary>
/// Complete order information with nested tenant and order assignments.
/// Includes 1-level nesting to avoid circular references.
/// </summary>
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
    string Status,
    TenantSummaryDto Tenant,
    IReadOnlyCollection<OrderAssignmentDto> Assignments
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
