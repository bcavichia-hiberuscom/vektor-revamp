using Hiberus.Industria.Vektor.Domain.Common.Interfaces;

namespace Hiberus.Industria.Vektor.Domain.OrderAssignment;

public sealed class OrderAssignment : IAuditable
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid VehicleId { get; private set; }
    public OrderAssignmentStatus Status { get; private set; }
    public DateTime AssignedAt { get; private set; }
    public DateTime? StartedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public DateTime? ActualArrival { get; private set; }
    public string? FailureReason { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public string CreatedBy { get; private set; } = string.Empty;
    public string? UpdatedBy { get; private set; }

    // Navigation properties
    public Order.Order Order { get; private set; } = null!;
    public Vehicle.Vehicle Vehicle { get; private set; } = null!;

    private OrderAssignment() { }

    public static OrderAssignment Create(Guid orderId, Guid vehicleId, string createdBy)
    {
        if (orderId == Guid.Empty)
            throw new ArgumentException("OrderId cannot be empty");
        if (vehicleId == Guid.Empty)
            throw new ArgumentException("VehicleId cannot be empty");

        return new OrderAssignment
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            VehicleId = vehicleId,
            Status = OrderAssignmentStatus.Pending,
            AssignedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy,
        };
    }

    public void Start(string updatedBy)
    {
        Status = OrderAssignmentStatus.InTransit;
        StartedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }

    public void Complete(DateTime actualArrival, string updatedBy)
    {
        Status = OrderAssignmentStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        ActualArrival = actualArrival;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }

    public void Fail(string reason, string updatedBy)
    {
        Status = OrderAssignmentStatus.Failed;
        FailureReason = reason;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }
}
