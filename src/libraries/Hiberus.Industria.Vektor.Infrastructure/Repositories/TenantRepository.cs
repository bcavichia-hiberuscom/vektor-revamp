using Hiberus.Industria.Vektor.Application.Common.Pagination;
using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.Tenant;
using Hiberus.Industria.Vektor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hiberus.Industria.Vektor.Infrastructure.Repositories;

public class TenantRepository : ITenantRepository
{
    private readonly VektorDbContext _context;

    public TenantRepository(VektorDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Query filter: Only active tenants (not soft-deleted).
    /// </summary>
    private IQueryable<Tenant> ActiveTenants =>
        _context.Tenants.AsNoTracking().Where(t => t.DeletedAt == null);

    /// <summary>
    /// Retrieves tenants with pagination, filtering only active tenants.
    /// Uses AsNoTracking() for read-only queries to improve performance.
    /// </summary>
    public async Task<(IEnumerable<Tenant> Items, int TotalCount)> GetAllPaginatedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var paginationParams = new PaginationParams(pageNumber, pageSize);

        // Get total count without loading data
        var totalCount = await ActiveTenants.CountAsync(ct);

        // Get paginated data
        var items = await ActiveTenants
            .OrderByDescending(t => t.CreatedAt)
            .Skip(paginationParams.GetSkip())
            .Take(paginationParams.PageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    /// <summary>
    /// Retrieves all active tenants. Use with caution - consider GetAllPaginatedAsync for large datasets.
    /// </summary>
    public async Task<IEnumerable<Tenant>> GetAllAsync(CancellationToken ct = default) =>
        await ActiveTenants.OrderByDescending(t => t.CreatedAt).ToListAsync(ct);

    /// <summary>
    /// Retrieves a single tenant by ID, filtering by active tenants only.
    /// </summary>
    public async Task<Tenant?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await ActiveTenants.FirstOrDefaultAsync(t => t.Id == id, ct);

    /// <summary>
    /// Retrieves a single tenant by slug, filtering by active tenants only.
    /// </summary>
    public async Task<Tenant?> GetBySlugAsync(string slug, CancellationToken ct = default) =>
        await ActiveTenants.FirstOrDefaultAsync(
            t => t.Slug == slug.Trim().ToLowerInvariant(),
            ct
        );

    public async Task<Tenant> CreateAsync(Tenant tenant, CancellationToken ct = default)
    {
        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync(ct);
        return tenant;
    }

    public async Task UpdateAsync(Tenant tenant, CancellationToken ct = default)
    {
        _context.Tenants.Update(tenant);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Id == id, ct);

        if (tenant is not null)
        {
            _context.Tenants.Remove(tenant);
            await _context.SaveChangesAsync(ct);
        }
    }

    /// <summary>
    /// Soft delete: Marks tenant as deleted without removing from database.
    /// </summary>
    public async Task SoftDeleteAsync(Tenant tenant, CancellationToken ct = default)
    {
        _context.Tenants.Update(tenant);
        await _context.SaveChangesAsync(ct);
    }
}
