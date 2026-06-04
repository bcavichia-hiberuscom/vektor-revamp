// Vektor.API/Domain/Entities/Order.cs
namespace Vektor.API.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string? ExternalOrderId { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public string Label { get; set; } = string.Empty;
    public string? Description { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerPhone { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Tenant Tenant { get; set; } = null!;
    public ICollection<OrderAssignment> Assignments { get; set; } = [];
}

public enum OrderStatus
{
    Pending,
    Assigned,
    InTransit,
    Completed,
    Cancelled,
}
