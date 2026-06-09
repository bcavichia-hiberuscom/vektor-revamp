using Hiberus.Industria.Vektor.Domain.OrderAssignment;

namespace Hiberus.Industria.Vektor.Application.Interfaces;

public interface IOrderAssignmentRepository
{
    Task<IEnumerable<OrderAssignment>> GetByTenantAsync(
        Guid tenantId,
        CancellationToken ct = default
    );
    Task<IEnumerable<OrderAssignment>> GetByOrderAsync(
        Guid orderId,
        Guid tenantId,
        CancellationToken ct = default
    );
    Task<IEnumerable<OrderAssignment>> GetByVehicleAsync(
        Guid vehicleId,
        Guid tenantId,
        CancellationToken ct = default
    );
    Task<OrderAssignment?> GetByIdAsync(Guid id, Guid tenantId, CancellationToken ct = default);
    Task<OrderAssignment> CreateAsync(OrderAssignment assignment, CancellationToken ct = default);
    Task UpdateAsync(OrderAssignment assignment, CancellationToken ct = default);
}
