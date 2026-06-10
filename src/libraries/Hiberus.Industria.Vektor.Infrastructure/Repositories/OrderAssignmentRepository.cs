using Hiberus.Industria.Vektor.Application.Common.Pagination;
using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.OrderAssignment;
using Hiberus.Industria.Vektor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hiberus.Industria.Vektor.Infrastructure.Repositories;

public class OrderAssignmentRepository : IOrderAssignmentRepository
{
    private readonly VektorDbContext _context;

    public OrderAssignmentRepository(VektorDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves order assignments with pagination, eager-loading related Order and Vehicle.
    /// Uses AsNoTracking() for read-only queries to improve performance.
    /// </summary>
    public async Task<(IEnumerable<OrderAssignment> Items, int TotalCount)> GetAllPaginatedAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var paginationParams = new PaginationParams(pageNumber, pageSize);

        // Get total count without loading data
        var totalCount = await _context
            .OrderAssignments.AsNoTracking()
            .Where(a => a.TenantId == tenantId)
            .CountAsync(ct);

        // Get paginated data with eager-loaded relations
        var items = await _context
            .OrderAssignments.AsNoTracking()
            .Where(a => a.TenantId == tenantId)
            .Include(a => a.Order)
            .Include(a => a.Vehicle)
            .OrderByDescending(a => a.AssignedAt)
            .Skip(paginationParams.GetSkip())
            .Take(paginationParams.PageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    /// <summary>
    /// Retrieves all order assignments for a tenant with eager-loaded relations.
    /// Use with caution - consider GetAllPaginatedAsync for large datasets.
    /// </summary>
    public async Task<IEnumerable<OrderAssignment>> GetByTenantAsync(
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .OrderAssignments.AsNoTracking()
            .Where(a => a.TenantId == tenantId)
            .Include(a => a.Order)
            .Include(a => a.Vehicle)
            .OrderByDescending(a => a.AssignedAt)
            .ToListAsync(ct);

    /// <summary>
    /// Retrieves order assignments for a specific order with eager-loaded relations.
    /// </summary>
    public async Task<IEnumerable<OrderAssignment>> GetByOrderAsync(
        Guid orderId,
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .OrderAssignments.AsNoTracking()
            .Where(a => a.OrderId == orderId && a.TenantId == tenantId)
            .Include(a => a.Order)
            .Include(a => a.Vehicle)
            .OrderByDescending(a => a.AssignedAt)
            .ToListAsync(ct);

    /// <summary>
    /// Retrieves order assignments for a specific vehicle with eager-loaded relations.
    /// </summary>
    public async Task<IEnumerable<OrderAssignment>> GetByVehicleAsync(
        Guid vehicleId,
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .OrderAssignments.AsNoTracking()
            .Where(a => a.VehicleId == vehicleId && a.TenantId == tenantId)
            .Include(a => a.Order)
            .Include(a => a.Vehicle)
            .OrderByDescending(a => a.AssignedAt)
            .ToListAsync(ct);

    /// <summary>
    /// Retrieves a single order assignment by ID with eager-loaded relations.
    /// </summary>
    public async Task<OrderAssignment?> GetByIdAsync(
        Guid id,
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .OrderAssignments.AsNoTracking()
            .Where(a => a.Id == id && a.TenantId == tenantId)
            .Include(a => a.Order)
            .Include(a => a.Vehicle)
            .FirstOrDefaultAsync(ct);

    public async Task<OrderAssignment> CreateAsync(
        OrderAssignment assignment,
        CancellationToken ct = default
    )
    {
        _context.OrderAssignments.Add(assignment);
        await _context.SaveChangesAsync(ct);
        return assignment;
    }

    public async Task UpdateAsync(OrderAssignment assignment, CancellationToken ct = default)
    {
        _context.OrderAssignments.Update(assignment);
        await _context.SaveChangesAsync(ct);
    }
}
