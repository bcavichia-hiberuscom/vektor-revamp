using Hiberus.Industria.Vektor.Domain.Tenant;

namespace Hiberus.Industria.Vektor.Application.Interfaces;

public interface ITenantRepository
{    /// <summary>
    /// Retrieves all tenants with pagination support.
    /// </summary>
    /// <returns>Tuple containing the collection of tenants and total count.</returns>
    Task<(IEnumerable<Tenant> Items, int TotalCount)> GetAllPaginatedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    );
    Task<IEnumerable<Tenant>> GetAllAsync(CancellationToken ct = default);
    Task<Tenant?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Tenant?> GetBySlugAsync(string slug, CancellationToken ct = default);
    Task<Tenant> CreateAsync(Tenant tenant, CancellationToken ct = default);
    Task UpdateAsync(Tenant tenant, CancellationToken ct = default);
    Task SoftDeleteAsync(Tenant tenant, CancellationToken ct = default);
}
