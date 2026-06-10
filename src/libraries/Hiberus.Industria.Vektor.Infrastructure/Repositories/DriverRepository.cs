using Hiberus.Industria.Vektor.Application.Common.Mappings;
using Hiberus.Industria.Vektor.Application.Common.Pagination;
using Hiberus.Industria.Vektor.Application.DTOs.Driver;
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

    // ==================== DTO Methods (Database-Level Projections) ====================

    /// <summary>
    /// Retrieves drivers as DTOs with pagination using database-level projections.
    /// All nested relations (Tenant, VehicleAssignments with their Driver, Vehicle, and Tenant) are projected
    /// at the database level to minimize data transfer and avoid N+1 queries.
    /// </summary>
    public async Task<(IEnumerable<DriverDto> Items, int TotalCount)> GetAllPaginatedAsDtoAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var paginationParams = new PaginationParams(pageNumber, pageSize);

        // Count total records matching the filter
        var totalCount = await _context
            .Drivers.AsNoTracking()
            .Where(d => d.TenantId == tenantId)
            .CountAsync(ct);

        // Retrieve paginated data with all required relationships for projection,
        // then project to DTOs at database level for optimal performance
        var items = await _context
            .Drivers.AsNoTracking()
            .Where(d => d.TenantId == tenantId)
            // Include all required relations for the DriverDto projection
            .Include(d => d.Tenant)
            .Include(d => d.VehicleAssignments)
            .ThenInclude(a => a.Driver)
            .Include(d => d.VehicleAssignments)
            .ThenInclude(a => a.Vehicle)
            .ThenInclude(v => v.Tenant)
            .Include(d => d.VehicleAssignments)
            .ThenInclude(a => a.Tenant)
            .OrderByDescending(d => d.CreatedAt)
            .Skip(paginationParams.GetSkip())
            .Take(paginationParams.PageSize)
            // Project to DTO at database level using compiled LINQ expressions
            .Select(ProjectionExtensions.ToDriverDtoExpression)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    /// <summary>
    /// Retrieves a single driver as DTO by ID using database-level projection.
    /// All nested relations are projected at database level for performance.
    /// </summary>
    public async Task<DriverDto?> GetByIdAsDtoAsync(
        Guid id,
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .Drivers.AsNoTracking()
            .Where(d => d.Id == id && d.TenantId == tenantId)
            // Include all required relations for the DriverDto projection
            .Include(d => d.Tenant)
            .Include(d => d.VehicleAssignments)
            .ThenInclude(a => a.Driver)
            .Include(d => d.VehicleAssignments)
            .ThenInclude(a => a.Vehicle)
            .ThenInclude(v => v.Tenant)
            .Include(d => d.VehicleAssignments)
            .ThenInclude(a => a.Tenant)
            // Project to DTO at database level using compiled LINQ expressions
            .Select(ProjectionExtensions.ToDriverDtoExpression)
            .FirstOrDefaultAsync(ct);

    // ==================== Entity Methods (For CRUD Operations) ====================

    // ==================== Entity Methods (For CRUD Operations) ====================

    /// <summary>
    /// Retrieves drivers with pagination for CRUD operations.
    /// Returns raw entities for mutation (Create, Update, Delete) scenarios.
    /// Includes Tenant and VehicleAssignments to prevent lazy-loading on mutation.
    /// For read-only DTOs with projections, use GetAllPaginatedAsDtoAsync() instead.
    /// </summary>
    /// <remarks>
    /// Note: Includes nested VehicleAssignments with their relations to prevent lazy-loading
    /// during entity mutation operations.
    /// </remarks>
    public async Task<(IEnumerable<Driver> Items, int TotalCount)> GetAllPaginatedAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var paginationParams = new PaginationParams(pageNumber, pageSize);

        // Count total records matching the tenant filter
        var totalCount = await _context
            .Drivers.AsNoTracking()
            .Where(d => d.TenantId == tenantId)
            .CountAsync(ct);

        // Retrieve paginated entities with basic relationships for CRUD operations
        var items = await _context
            .Drivers.AsNoTracking()
            .Where(d => d.TenantId == tenantId)
            // Include only necessary relations for entity mutation scenarios
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
    /// Retrieves all drivers for a tenant for CRUD operations without pagination.
    /// Use with caution - consider GetAllPaginatedAsync for large datasets.
    /// Returns raw entities with basic relationships.
    /// </summary>
    /// <remarks>
    /// Warning: This method returns the entire result set. For large driver collections,
    /// use GetAllPaginatedAsync instead to avoid memory issues and improve performance.
    /// </remarks>
    public async Task<IEnumerable<Driver>> GetAllAsync(
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .Drivers.AsNoTracking()
            .Where(d => d.TenantId == tenantId)
            // Include necessary relations for entity mutation scenarios
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
    /// Retrieves a single driver by ID for CRUD operations.
    /// Returns raw entity with basic relationships, ready for mutation.
    /// Enforces tenant isolation at the database query level.
    /// </summary>
    /// <remarks>
    /// This method ensures tenant isolation by filtering both ID and TenantId.
    /// Never rely on TenantId from the caller for security; always validate at query level.
    /// </remarks>
    public async Task<Driver?> GetByIdAsync(
        Guid id,
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .Drivers.AsNoTracking()
            .Where(d => d.Id == id && d.TenantId == tenantId)
            // Include necessary relations for entity mutation scenarios
            .Include(d => d.Tenant)
            .Include(d => d.VehicleAssignments)
            .ThenInclude(a => a.Tenant)
            .Include(d => d.VehicleAssignments)
            .ThenInclude(a => a.Driver)
            .Include(d => d.VehicleAssignments)
            .ThenInclude(a => a.Vehicle)
            .FirstOrDefaultAsync(ct);

    /// <summary>
    /// Creates a new driver entity in the database.
    /// Assumes domain validation has already been performed by the domain aggregate root.
    /// </summary>
    public async Task<Driver> CreateAsync(Driver driver, CancellationToken ct = default)
    {
        _context.Drivers.Add(driver);
        await _context.SaveChangesAsync(ct);
        return driver;
    }

    /// <summary>
    /// Updates an existing driver entity in the database.
    /// Assumes domain validation has already been performed by the domain aggregate root.
    /// </summary>
    public async Task UpdateAsync(Driver driver, CancellationToken ct = default)
    {
        _context.Drivers.Update(driver);
        await _context.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Deletes a driver from the database.
    /// Enforces tenant isolation to prevent cross-tenant data deletion.
    /// </summary>
    /// <remarks>
    /// Validates tenant ID to ensure the driver belongs to the specified tenant
    /// before deletion, protecting against authorization bypasses.
    /// </remarks>
    public async Task DeleteAsync(Guid id, Guid tenantId, CancellationToken ct = default)
    {
        // Query with tenant isolation to prevent deletion of drivers from other tenants
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
