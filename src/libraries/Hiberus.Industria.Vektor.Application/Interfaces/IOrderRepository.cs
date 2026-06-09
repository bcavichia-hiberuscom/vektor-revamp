using Hiberus.Industria.Vektor.Domain.Order;

namespace Hiberus.Industria.Vektor.Application.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync(Guid tenantId, CancellationToken ct = default);
    Task<Order?> GetByIdAsync(Guid id, Guid tenantId, CancellationToken ct = default);
    Task<Order> CreateAsync(Order order, CancellationToken ct = default);
    Task UpdateAsync(Order order, CancellationToken ct = default);
    Task DeleteAsync(Guid id, Guid tenantId, CancellationToken ct = default);
}
