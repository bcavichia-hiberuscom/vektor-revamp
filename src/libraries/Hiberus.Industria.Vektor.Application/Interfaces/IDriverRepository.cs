using Hiberus.Industria.Vektor.Application.DTOs.Driver;
using Hiberus.Industria.Vektor.Domain.Driver;

namespace Hiberus.Industria.Vektor.Application.Interfaces;

public interface IDriverRepository
{
    // ==================== DTO Methods (Optimized Query Projections) ====================

    /// <summary>
    /// Retrieves drivers as DTOs with pagination, with database-level projections.
    /// All nested relations are projected at the database level for optimal performance.
    /// </summary>
    Task<(IEnumerable<DriverDto> Items, int TotalCount)> GetAllPaginatedAsDtoAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    );

    /// <summary>
    /// Retrieves a single driver as DTO by ID with database-level projections.
    /// </summary>
    Task<DriverDto?> GetByIdAsDtoAsync(Guid id, Guid tenantId, CancellationToken ct = default);

    // ==================== Entity Methods (For CRUD Operations) ====================

    /// <summary>
    /// Retrieves all drivers for a given tenant with pagination support.
    /// </summary>
    /// <returns>Tuple containing the collection of drivers and total count of drivers.</returns>
    Task<(IEnumerable<Driver> Items, int TotalCount)> GetAllPaginatedAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    );

    Task<IEnumerable<Driver>> GetAllAsync(Guid tenantId, CancellationToken ct = default);
    Task<Driver?> GetByIdAsync(Guid id, Guid tenantId, CancellationToken ct = default);
    Task<Driver> CreateAsync(Driver driver, CancellationToken ct = default);
    Task UpdateAsync(Driver driver, CancellationToken ct = default);
    Task DeleteAsync(Guid id, Guid tenantId, CancellationToken ct = default);
}
