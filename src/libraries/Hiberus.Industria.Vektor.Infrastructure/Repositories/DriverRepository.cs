using Hiberus.Industria.Vektor.Application.Common.Pagination;
using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.Driver;
using Hiberus.Industria.Vektor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hiberus.Industria.Vektor.Infrastructure.Repositories;

public class DriverRepository : IDriverRepository
{
    private readonly VektorDbContext _context;

    public DriverRepository(VektorDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves drivers with pagination, eager-loading related Tenant.
    /// Uses AsNoTracking() for read-only queries to improve performance.
    /// </summary>
    public async Task<(IEnumerable<Driver> Items, int TotalCount)> GetAllPaginatedAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var paginationParams = new PaginationParams(pageNumber, pageSize);

        // Get total count without loading data
        var totalCount = await _context
            .Drivers.AsNoTracking()
            .Where(d => d.TenantId == tenantId)
            .CountAsync(ct);

        // Get paginated data with eager-loaded relations
        var items = await _context
            .Drivers.AsNoTracking()
            .Where(d => d.TenantId == tenantId)
            .Include(d => d.Tenant)
            .Include(d => d.VehicleAssignments)
            .ThenInclude(a => a.Tenant)
            .Include(d => d.VehicleAssignments)
            .ThenInclude(a => a.Driver)
            .Include(d => d.VehicleAssignments)
            .ThenInclude(a => a.Vehicle)
            .OrderByDescending(d => d.CreatedAt)
            .Skip(paginationParams.GetSkip())
            .Take(paginationParams.PageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    /// <summary>
    /// Retrieves all drivers for a tenant with eager-loaded relations.
    /// Use with caution - consider GetAllPaginatedAsync for large datasets.
    /// </summary>
    public async Task<IEnumerable<Driver>> GetAllAsync(
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .Drivers.AsNoTracking()
            .Where(d => d.TenantId == tenantId)
            .Include(d => d.Tenant)
            .Include(d => d.VehicleAssignments)
            .ThenInclude(a => a.Tenant)
            .Include(d => d.VehicleAssignments)
            .ThenInclude(a => a.Driver)
            .Include(d => d.VehicleAssignments)
            .ThenInclude(a => a.Vehicle)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(ct);

    /// <summary>
    /// Retrieves a single driver by ID with eager-loaded relations.
    /// </summary>
    public async Task<Driver?> GetByIdAsync(
        Guid id,
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .Drivers.AsNoTracking()
            .Where(d => d.Id == id && d.TenantId == tenantId)
            .Include(d => d.Tenant)
            .Include(d => d.VehicleAssignments)
            .ThenInclude(a => a.Tenant)
            .Include(d => d.VehicleAssignments)
            .ThenInclude(a => a.Driver)
            .Include(d => d.VehicleAssignments)
            .ThenInclude(a => a.Vehicle)
            .FirstOrDefaultAsync(ct);

    public async Task<Driver> CreateAsync(Driver driver, CancellationToken ct = default)
    {
        _context.Drivers.Add(driver);
        await _context.SaveChangesAsync(ct);
        return driver;
    }

    public async Task UpdateAsync(Driver driver, CancellationToken ct = default)
    {
        _context.Drivers.Update(driver);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, Guid tenantId, CancellationToken ct = default)
    {
        var driver = await _context.Drivers.FirstOrDefaultAsync(
            d => d.Id == id && d.TenantId == tenantId,
            ct
        );

        if (driver is not null)
        {
            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync(ct);
        }
    }
}
