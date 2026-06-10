using Hiberus.Industria.Vektor.Application.DTOs.Tenant;
using Hiberus.Industria.Vektor.Domain.Tenant;

namespace Hiberus.Industria.Vektor.Application.Interfaces;

public interface ITenantRepository
{
    // ==================== DTO Methods (Optimized Query Projections) ====================

    /// <summary>
    /// Retrieves tenants as DTOs with pagination, with database-level projections.
    /// </summary>
    Task<(IEnumerable<TenantDto> Items, int TotalCount)> GetAllPaginatedAsDtoAsync(
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    );

    /// <summary>
    /// Retrieves a single tenant as DTO by ID with database-level projection.
    /// </summary>
    Task<TenantDto?> GetByIdAsDtoAsync(Guid id, CancellationToken ct = default);

    // ==================== Entity Methods (For CRUD Operations) ====================

    /// <summary>
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
