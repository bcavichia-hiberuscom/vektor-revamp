using Hiberus.Industria.Vektor.Domain.Common.Interfaces;

namespace Hiberus.Industria.Vektor.Domain.Order;

public sealed class Order : IAuditable
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string? ExternalOrderId { get; private set; }
    public OrderStatus Status { get; private set; }
    public string Label { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public string? CustomerName { get; private set; }
    public string? CustomerPhone { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public string CreatedBy { get; private set; } = string.Empty;
    public string? UpdatedBy { get; private set; }

    // Navigation properties
    public Tenant.Tenant Tenant { get; private set; } = null!;
    public IReadOnlyCollection<OrderAssignment.OrderAssignment> Assignments { get; private set; } =
    [];

    private Order() { }

    public static Order Create(
        Guid tenantId,
        string label,
        double latitude,
        double longitude,
        string createdBy,
        string? externalOrderId = null,
        string? description = null,
        string? customerName = null,
        string? customerPhone = null
    )
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId cannot be empty");
        if (string.IsNullOrWhiteSpace(label))
            throw new ArgumentException("Label is required");

        return new Order
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Label = label.Trim(),
            Latitude = latitude,
            Longitude = longitude,
            ExternalOrderId = externalOrderId,
            Description = description?.Trim(),
            CustomerName = customerName?.Trim(),
            CustomerPhone = customerPhone?.Trim(),
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy,
        };
    }

    public void Cancel(string updatedBy)
    {
        if (Status == OrderStatus.Completed)
            throw new InvalidOperationException("Cannot cancel a completed order");

        Status = OrderStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }
}
