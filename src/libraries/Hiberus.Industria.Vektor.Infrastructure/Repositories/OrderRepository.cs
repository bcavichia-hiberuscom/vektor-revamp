using Hiberus.Industria.Vektor.Application.Common.Pagination;
using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.Order;
using Hiberus.Industria.Vektor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hiberus.Industria.Vektor.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly VektorDbContext _context;

    public OrderRepository(VektorDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves orders with pagination, eager-loading related Tenant.
    /// Uses AsNoTracking() for read-only queries to improve performance.
    /// </summary>
    public async Task<(IEnumerable<Order> Items, int TotalCount)> GetAllPaginatedAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var paginationParams = new PaginationParams(pageNumber, pageSize);

        // Get total count without loading data
        var totalCount = await _context.Orders
            .AsNoTracking()
            .Where(o => o.TenantId == tenantId)
            .CountAsync(ct);

        // Get paginated data with eager-loaded relations
        var items = await _context.Orders
            .AsNoTracking()
            .Where(o => o.TenantId == tenantId)
            .Include(o => o.Tenant)
            .Include(o => o.Assignments)
            .OrderByDescending(o => o.CreatedAt)
            .Skip(paginationParams.GetSkip())
            .Take(paginationParams.PageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    /// <summary>
    /// Retrieves all orders for a tenant with eager-loaded relations.
    /// Use with caution - consider GetAllPaginatedAsync for large datasets.
    /// </summary>
    public async Task<IEnumerable<Order>> GetAllAsync(
        Guid tenantId,
        CancellationToken ct = default
    ) => await _context.Orders
        .AsNoTracking()
        .Where(o => o.TenantId == tenantId)
        .Include(o => o.Tenant)
        .Include(o => o.Assignments)
        .OrderByDescending(o => o.CreatedAt)
        .ToListAsync(ct);

    /// <summary>
    /// Retrieves a single order by ID with eager-loaded relations.
    /// </summary>
    public async Task<Order?> GetByIdAsync(
        Guid id,
        Guid tenantId,
        CancellationToken ct = default
    ) => await _context.Orders
        .AsNoTracking()
        .Where(o => o.Id == id && o.TenantId == tenantId)
        .Include(o => o.Tenant)
        .Include(o => o.Assignments)
        .FirstOrDefaultAsync(ct);

    public async Task<Order> CreateAsync(Order order, CancellationToken ct = default)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync(ct);
        return order;
    }

    public async Task UpdateAsync(Order order, CancellationToken ct = default)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, Guid tenantId, CancellationToken ct = default)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(
            o => o.Id == id && o.TenantId == tenantId,
            ct
        );

        if (order is not null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync(ct);
        }
    }
}
