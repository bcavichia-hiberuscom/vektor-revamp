using Hiberus.Industria.Vektor.Application.Common.Pagination;
using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.Vehicle;
using Hiberus.Industria.Vektor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hiberus.Industria.Vektor.Infrastructure.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly VektorDbContext _context;

    public VehicleRepository(VektorDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves vehicles with pagination, eager-loading related Tenant and Assignments.
    /// Uses AsNoTracking() for read-only queries to improve performance.
    /// </summary>
    public async Task<(IEnumerable<Vehicle> Items, int TotalCount)> GetAllPaginatedAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var paginationParams = new PaginationParams(pageNumber, pageSize);

        // Get total count without loading data
        var totalCount = await _context.Vehicles
            .AsNoTracking()
            .Where(v => v.TenantId == tenantId)
            .CountAsync(ct);

        // Get paginated data with eager-loaded relations
        var items = await _context.Vehicles
            .AsNoTracking()
            .Where(v => v.TenantId == tenantId)
            .Include(v => v.Tenant)
            .Include(v => v.Assignments)
                .ThenInclude(a => a.Tenant)
            .Include(v => v.Assignments)
                .ThenInclude(a => a.Driver)
            .Include(v => v.Assignments)
                .ThenInclude(a => a.Vehicle)
            .Include(v => v.OrderAssignments)
                .ThenInclude(oa => oa.Order)
            .Include(v => v.OrderAssignments)
                .ThenInclude(oa => oa.Tenant)
            .OrderByDescending(v => v.CreatedAt)
            .Skip(paginationParams.GetSkip())
            .Take(paginationParams.PageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    /// <summary>
    /// Retrieves all vehicles for a tenant with eager-loaded relations.
    /// Use with caution - consider GetAllPaginatedAsync for large datasets.
    /// </summary>
    public async Task<IEnumerable<Vehicle>> GetAllAsync(
        Guid tenantId,
        CancellationToken ct = default
    ) => await _context.Vehicles
        .AsNoTracking()
        .Where(v => v.TenantId == tenantId)
        .Include(v => v.Tenant)
        .Include(v => v.Assignments)
            .ThenInclude(a => a.Tenant)
        .Include(v => v.Assignments)
            .ThenInclude(a => a.Driver)
        .Include(v => v.Assignments)
            .ThenInclude(a => a.Vehicle)
        .Include(v => v.OrderAssignments)
            .ThenInclude(oa => oa.Order)
        .Include(v => v.OrderAssignments)
            .ThenInclude(oa => oa.Tenant)
        .OrderByDescending(v => v.CreatedAt)
        .ToListAsync(ct);

    /// <summary>
    /// Retrieves a single vehicle by ID with eager-loaded relations.
    /// </summary>
    public async Task<Vehicle?> GetByIdAsync(
        Guid id,
        Guid tenantId,
        CancellationToken ct = default
    ) => await _context.Vehicles
        .AsNoTracking()
        .Where(v => v.Id == id && v.TenantId == tenantId)
        .Include(v => v.Tenant)
        .Include(v => v.Assignments)
            .ThenInclude(a => a.Tenant)
        .Include(v => v.Assignments)
            .ThenInclude(a => a.Driver)
        .Include(v => v.Assignments)
            .ThenInclude(a => a.Vehicle)
        .Include(v => v.OrderAssignments)
            .ThenInclude(oa => oa.Order)
        .Include(v => v.OrderAssignments)
            .ThenInclude(oa => oa.Tenant)
        .FirstOrDefaultAsync(ct);

    public async Task<Vehicle> CreateAsync(Vehicle vehicle, CancellationToken ct = default)
    {
        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync(ct);
        return vehicle;
    }

    public async Task UpdateAsync(Vehicle vehicle, CancellationToken ct = default)
    {
        _context.Vehicles.Update(vehicle);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, Guid tenantId, CancellationToken ct = default)
    {
        var vehicle = await _context.Vehicles.FirstOrDefaultAsync(
            v => v.Id == id && v.TenantId == tenantId,
            ct
        );

        if (vehicle is not null)
        {
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync(ct);
        }
    }
}
