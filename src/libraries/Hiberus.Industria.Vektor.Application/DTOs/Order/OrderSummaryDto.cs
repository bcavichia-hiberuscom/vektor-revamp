namespace Hiberus.Industria.Vektor.Application.DTOs.Order;

/// <summary>
/// Minimal order information without collections to prevent circular references.
/// Used for nested references in other DTOs.
/// </summary>
public record OrderSummaryDto(Guid Id, string Label, string Status, string? CustomerName);
