using Hiberus.Industria.Vektor.Application.Common.Pagination;
using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.DriverVehicleAssignment;
using Hiberus.Industria.Vektor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hiberus.Industria.Vektor.Infrastructure.Repositories;

public class DriverVehicleAssignmentRepository : IDriverVehicleAssignmentRepository
{
    private readonly VektorDbContext _context;

    public DriverVehicleAssignmentRepository(VektorDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves driver-vehicle assignments with pagination, eager-loading Driver and Vehicle.
    /// Uses AsNoTracking() for read-only queries to improve performance.
    /// </summary>
    public async Task<(
        IEnumerable<DriverVehicleAssignment> Items,
        int TotalCount
    )> GetAllPaginatedAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var paginationParams = new PaginationParams(pageNumber, pageSize);

        // Get total count without loading data
        var totalCount = await _context
            .DriverVehicleAssignments.AsNoTracking()
            .Where(a => a.TenantId == tenantId)
            .CountAsync(ct);

        // Get paginated data with eager-loaded relations
        var items = await _context
            .DriverVehicleAssignments.AsNoTracking()
            .Where(a => a.TenantId == tenantId)
            .Include(a => a.Driver)
            .Include(a => a.Vehicle)
            .OrderByDescending(a => a.AssignedAt)
            .Skip(paginationParams.GetSkip())
            .Take(paginationParams.PageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    /// <summary>
    /// Retrieves all driver-vehicle assignments for a tenant with eager-loaded relations.
    /// Use with caution - consider GetAllPaginatedAsync for large datasets.
    /// </summary>
    public async Task<IEnumerable<DriverVehicleAssignment>> GetByTenantAsync(
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .DriverVehicleAssignments.AsNoTracking()
            .Where(a => a.TenantId == tenantId)
            .Include(a => a.Driver)
            .Include(a => a.Vehicle)
            .OrderByDescending(a => a.AssignedAt)
            .ToListAsync(ct);

    /// <summary>
    /// Retrieves a single assignment by ID with eager-loaded relations.
    /// </summary>
    public async Task<DriverVehicleAssignment?> GetByIdAsync(
        Guid id,
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .DriverVehicleAssignments.AsNoTracking()
            .Where(a => a.Id == id && a.TenantId == tenantId)
            .Include(a => a.Driver)
            .Include(a => a.Vehicle)
            .FirstOrDefaultAsync(ct);

    /// <summary>
    /// Retrieves assignments for a specific driver with eager-loaded relations.
    /// </summary>
    public async Task<IEnumerable<DriverVehicleAssignment>> GetByDriverAsync(
        Guid driverId,
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .DriverVehicleAssignments.AsNoTracking()
            .Where(a => a.DriverId == driverId && a.TenantId == tenantId)
            .Include(a => a.Driver)
            .Include(a => a.Vehicle)
            .OrderByDescending(a => a.AssignedAt)
            .ToListAsync(ct);

    /// <summary>
    /// Retrieves assignments for a specific vehicle with eager-loaded relations.
    /// </summary>
    public async Task<IEnumerable<DriverVehicleAssignment>> GetByVehicleAsync(
        Guid vehicleId,
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .DriverVehicleAssignments.AsNoTracking()
            .Where(a => a.VehicleId == vehicleId && a.TenantId == tenantId)
            .Include(a => a.Driver)
            .Include(a => a.Vehicle)
            .OrderByDescending(a => a.AssignedAt)
            .ToListAsync(ct);

    public async Task<DriverVehicleAssignment> CreateAsync(
        DriverVehicleAssignment assignment,
        CancellationToken ct = default
    )
    {
        _context.DriverVehicleAssignments.Add(assignment);
        await _context.SaveChangesAsync(ct);
        return assignment;
    }

    public async Task UpdateAsync(
        DriverVehicleAssignment assignment,
        CancellationToken ct = default
    )
    {
        _context.DriverVehicleAssignments.Update(assignment);
        await _context.SaveChangesAsync(ct);
    }
}
