using Hiberus.Industria.Vektor.Domain.Driver;

namespace Hiberus.Industria.Vektor.Application.Interfaces;

public interface IDriverRepository
{
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
