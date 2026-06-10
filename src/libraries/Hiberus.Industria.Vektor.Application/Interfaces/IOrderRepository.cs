using Hiberus.Industria.Vektor.Domain.Order;

namespace Hiberus.Industria.Vektor.Application.Interfaces;

public interface IOrderRepository
{
    /// <summary>
    /// Retrieves all orders for a given tenant with pagination support.
    /// </summary>
    /// <returns>Tuple containing the collection of orders and total count of orders.</returns>
    Task<(IEnumerable<Order> Items, int TotalCount)> GetAllPaginatedAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    );

    Task<IEnumerable<Order>> GetAllAsync(Guid tenantId, CancellationToken ct = default);
    Task<Order?> GetByIdAsync(Guid id, Guid tenantId, CancellationToken ct = default);
    Task<Order> CreateAsync(Order order, CancellationToken ct = default);
    Task UpdateAsync(Order order, CancellationToken ct = default);
    Task DeleteAsync(Guid id, Guid tenantId, CancellationToken ct = default);
}
