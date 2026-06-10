using Hiberus.Industria.Vektor.Application.Common.Mappings;
using Hiberus.Industria.Vektor.Application.Common.Pagination;
using Hiberus.Industria.Vektor.Application.DTOs.Order;
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

    // ==================== DTO Methods (Database-Level Projections) ====================

    /// <summary>
    /// Retrieves orders as DTOs with pagination using database-level projections.
    /// All nested relations (Tenant, Assignments with their Vehicle and Tenant) are projected
    /// at the database level to minimize data transfer and avoid N+1 queries.
    /// </summary>
    public async Task<(IEnumerable<OrderDto> Items, int TotalCount)> GetAllPaginatedAsDtoAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var paginationParams = new PaginationParams(pageNumber, pageSize);

        // Count total records matching the filter
        var totalCount = await _context
            .Orders.AsNoTracking()
            .Where(o => o.TenantId == tenantId)
            .CountAsync(ct);

        // Retrieve paginated data with all required relationships for projection,
        // then project to DTOs at database level for optimal performance
        var items = await _context
            .Orders.AsNoTracking()
            .Where(o => o.TenantId == tenantId)
            // Include all required relations for the OrderDto projection
            .Include(o => o.Tenant)
            .Include(o => o.Assignments)
            .ThenInclude(a => a.Order)
            .Include(o => o.Assignments)
            .ThenInclude(a => a.Vehicle)
            .ThenInclude(v => v.Tenant)
            .Include(o => o.Assignments)
            .ThenInclude(a => a.Tenant)
            .OrderByDescending(o => o.CreatedAt)
            .Skip(paginationParams.GetSkip())
            .Take(paginationParams.PageSize)
            // Project to DTO at database level using compiled LINQ expressions
            .Select(ProjectionExtensions.ToOrderDtoExpression)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    /// <summary>
    /// Retrieves a single order as DTO by ID using database-level projection.
    /// All nested relations are projected at database level for performance.
    /// </summary>
    public async Task<OrderDto?> GetByIdAsDtoAsync(
        Guid id,
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .Orders.AsNoTracking()
            .Where(o => o.Id == id && o.TenantId == tenantId)
            // Include all required relations for the OrderDto projection
            .Include(o => o.Tenant)
            .Include(o => o.Assignments)
            .ThenInclude(a => a.Order)
            .Include(o => o.Assignments)
            .ThenInclude(a => a.Vehicle)
            .ThenInclude(v => v.Tenant)
            .Include(o => o.Assignments)
            .ThenInclude(a => a.Tenant)
            // Project to DTO at database level using compiled LINQ expressions
            .Select(ProjectionExtensions.ToOrderDtoExpression)
            .FirstOrDefaultAsync(ct);

    // ==================== Entity Methods (For CRUD Operations) ====================

    /// <summary>
    /// Retrieves orders with pagination for CRUD operations.
    /// Returns raw entities for mutation (Create, Update, Delete) scenarios.
    /// Includes Tenant and Assignments to prevent lazy-loading on mutation.
    /// For read-only DTOs with projections, use GetAllPaginatedAsDtoAsync() instead.
    /// </summary>
    /// <remarks>
    /// Note: Does not include nested Order, Vehicle in Assignments since those
    /// are primarily used for DTO projections. CRUD operations typically work
    /// with single-level entity relationships.
    /// </remarks>
    public async Task<(IEnumerable<Order> Items, int TotalCount)> GetAllPaginatedAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var paginationParams = new PaginationParams(pageNumber, pageSize);

        // Count total records matching the tenant filter
        var totalCount = await _context
            .Orders.AsNoTracking()
            .Where(o => o.TenantId == tenantId)
            .CountAsync(ct);

        // Retrieve paginated entities with basic relationships for CRUD operations
        var items = await _context
            .Orders.AsNoTracking()
            .Where(o => o.TenantId == tenantId)
            // Include only necessary relations for entity mutation scenarios
            .Include(o => o.Tenant)
            .Include(o => o.Assignments)
            .OrderByDescending(o => o.CreatedAt)
            .Skip(paginationParams.GetSkip())
            .Take(paginationParams.PageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    /// <summary>
    /// Retrieves all orders for a tenant for CRUD operations without pagination.
    /// Use with caution - consider GetAllPaginatedAsync for large datasets.
    /// Returns raw entities with basic relationships.
    /// </summary>
    /// <remarks>
    /// Warning: This method returns the entire result set. For large order collections,
    /// use GetAllPaginatedAsync instead to avoid memory issues and improve performance.
    /// </remarks>
    public async Task<IEnumerable<Order>> GetAllAsync(
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .Orders.AsNoTracking()
            .Where(o => o.TenantId == tenantId)
            // Include necessary relations for entity mutation scenarios
            .Include(o => o.Tenant)
            .Include(o => o.Assignments)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(ct);

    /// <summary>
    /// Retrieves a single order by ID for CRUD operations.
    /// Returns raw entity with basic relationships, ready for mutation.
    /// Enforces tenant isolation at the database query level.
    /// </summary>
    /// <remarks>
    /// This method ensures tenant isolation by filtering both ID and TenantId.
    /// Never rely on TenantId from the caller for security; always validate at query level.
    /// </remarks>
    public async Task<Order?> GetByIdAsync(
        Guid id,
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .Orders.AsNoTracking()
            .Where(o => o.Id == id && o.TenantId == tenantId)
            // Include necessary relations for entity mutation scenarios
            .Include(o => o.Tenant)
            .Include(o => o.Assignments)
            .FirstOrDefaultAsync(ct);

    /// <summary>
    /// Creates a new order entity in the database.
    /// Assumes domain validation has already been performed by the domain aggregate root.
    /// </summary>
    public async Task<Order> CreateAsync(Order order, CancellationToken ct = default)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync(ct);
        return order;
    }

    /// <summary>
    /// Updates an existing order entity in the database.
    /// Assumes domain validation has already been performed by the domain aggregate root.
    /// </summary>
    public async Task UpdateAsync(Order order, CancellationToken ct = default)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Deletes an order from the database.
    /// Enforces tenant isolation to prevent cross-tenant data deletion.
    /// </summary>
    /// <remarks>
    /// Validates tenant ID to ensure the order belongs to the specified tenant
    /// before deletion, protecting against authorization bypasses.
    /// </remarks>
    public async Task DeleteAsync(Guid id, Guid tenantId, CancellationToken ct = default)
    {
        // Query with tenant isolation to prevent deletion of orders from other tenants
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
