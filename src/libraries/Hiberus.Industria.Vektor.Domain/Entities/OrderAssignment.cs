namespace Hiberus.Industria.Vektor.Domain.Entities;

public class OrderAssignment
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid VehicleId { get; set; }
    public OrderAssignmentStatus Status { get; set; } = OrderAssignmentStatus.Pending;
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? ActualArrival { get; set; }
    public string? FailureReason { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Order Order { get; set; } = null!;
    public Vehicle Vehicle { get; set; } = null!;
}

public enum OrderAssignmentStatus
{
    Pending,
    InTransit,
    Completed,
    Failed,
    Recovered,
}
