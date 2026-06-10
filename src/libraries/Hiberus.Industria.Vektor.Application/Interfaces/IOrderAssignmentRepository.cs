using Hiberus.Industria.Vektor.Application.DTOs.OrderAssignment;
using Hiberus.Industria.Vektor.Domain.OrderAssignment;

namespace Hiberus.Industria.Vektor.Application.Interfaces;

public interface IOrderAssignmentRepository
{
    /// <summary>
    /// Retrieves all order assignments for a given tenant with pagination support.
    /// </summary>
    /// <returns>Tuple containing the collection of assignments and total count.</returns>
    Task<(IEnumerable<OrderAssignment> Items, int TotalCount)> GetAllPaginatedAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    );

    /// <summary>
    /// Retrieves all order assignments for a given tenant as DTOs with pagination support.
    /// Uses database-level projection to minimize data transfer.
    /// </summary>
    /// <returns>Tuple containing the collection of OrderAssignmentDto and total count.</returns>
    Task<(IEnumerable<OrderAssignmentDto> Items, int TotalCount)> GetAllPaginatedAsDtoAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    );

    /// <summary>
    /// Retrieves a single order assignment by ID as DTO with eager-loaded relations.
    /// Uses database-level projection for optimal performance.
    /// </summary>
    Task<OrderAssignmentDto?> GetByIdAsDtoAsync(Guid id, Guid tenantId, CancellationToken ct = default);

    Task<IEnumerable<OrderAssignment>> GetByTenantAsync(
        Guid tenantId,
        CancellationToken ct = default
    );
    Task<IEnumerable<OrderAssignment>> GetByOrderAsync(
        Guid orderId,
        Guid tenantId,
        CancellationToken ct = default
    );
    Task<IEnumerable<OrderAssignmentDto>> GetByOrderAsDtoAsync(
        Guid orderId,
        Guid tenantId,
        CancellationToken ct = default
    );
    Task<IEnumerable<OrderAssignment>> GetByVehicleAsync(
        Guid vehicleId,
        Guid tenantId,
        CancellationToken ct = default
    );
    Task<IEnumerable<OrderAssignmentDto>> GetByVehicleAsDtoAsync(
        Guid vehicleId,
        Guid tenantId,
        CancellationToken ct = default
    );
    Task<OrderAssignment?> GetByIdAsync(Guid id, Guid tenantId, CancellationToken ct = default);
    Task<OrderAssignment> CreateAsync(OrderAssignment assignment, CancellationToken ct = default);
    Task UpdateAsync(OrderAssignment assignment, CancellationToken ct = default);
}
