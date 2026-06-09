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

    private IQueryable<Tenant> ActiveTenants => _context.Tenants.Where(t => t.DeletedAt == null);

    public async Task<IEnumerable<Tenant>> GetAllAsync(CancellationToken ct = default) =>
        await ActiveTenants.ToListAsync(ct);

    public async Task<Tenant?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await ActiveTenants.FirstOrDefaultAsync(t => t.Id == id, ct);

    public async Task<Tenant?> GetBySlugAsync(string slug, CancellationToken ct = default) =>
        await ActiveTenants.FirstOrDefaultAsync(t => t.Slug == slug.Trim().ToLowerInvariant(), ct);

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

    public async Task SoftDeleteAsync(Tenant tenant, CancellationToken ct = default)
    {
        _context.Tenants.Update(tenant);
        await _context.SaveChangesAsync(ct);
    }
}
