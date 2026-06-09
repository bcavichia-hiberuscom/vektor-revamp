using ErrorOr;
using Hiberus.Industria.Vektor.Domain.Common.Interfaces;

namespace Hiberus.Industria.Vektor.Domain.OrderAssignment;

public sealed class OrderAssignment : IAuditable
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
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
    public Tenant.Tenant Tenant { get; private set; } = null!;
    public Order.Order Order { get; private set; } = null!;
    public Vehicle.Vehicle Vehicle { get; private set; } = null!;

    private OrderAssignment() { }

    public static ErrorOr<OrderAssignment> Create(
        Guid tenantId,
        Guid orderId,
        Guid vehicleId,
        string createdBy
    )
    {
        if (tenantId == Guid.Empty)
            return Error.Validation("OrderAssignment.TenantId", "TenantId cannot be empty");
        if (orderId == Guid.Empty)
            return Error.Validation("OrderAssignment.OrderId", "OrderId cannot be empty");
        if (vehicleId == Guid.Empty)
            return Error.Validation("OrderAssignment.VehicleId", "VehicleId cannot be empty");

        return new OrderAssignment
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            OrderId = orderId,
            VehicleId = vehicleId,
            Status = OrderAssignmentStatus.Pending,
            AssignedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy,
        };
    }

    public ErrorOr<OrderAssignment> Start(string updatedBy)
    {
        if (Status != OrderAssignmentStatus.Pending)
            return Error.Conflict("OrderAssignment.Status", "Assignment is not in pending state");

        Status = OrderAssignmentStatus.InTransit;
        StartedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;

        return this;
    }

    public ErrorOr<OrderAssignment> Complete(DateTime actualArrival, string updatedBy)
    {
        if (Status != OrderAssignmentStatus.InTransit)
            return Error.Conflict("OrderAssignment.Status", "Assignment is not in transit");

        Status = OrderAssignmentStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        ActualArrival = actualArrival;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;

        return this;
    }

    public ErrorOr<OrderAssignment> Fail(string reason, string updatedBy)
    {
        if (string.IsNullOrWhiteSpace(reason))
            return Error.Validation("OrderAssignment.FailureReason", "Failure reason is required");

        Status = OrderAssignmentStatus.Failed;
        FailureReason = reason;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;

        return this;
    }
}
