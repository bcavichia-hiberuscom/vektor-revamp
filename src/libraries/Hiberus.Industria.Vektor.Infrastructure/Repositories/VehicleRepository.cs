using Hiberus.Industria.Vektor.Application.Common.Mappings;
using Hiberus.Industria.Vektor.Application.Common.Pagination;
using Hiberus.Industria.Vektor.Application.DTOs.Vehicle;
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

    // ==================== DTO Methods (Database-Level Projections) ====================

    /// <summary>
    /// Retrieves vehicles as DTOs with pagination using database-level projections.
    /// All nested relations (Tenant, Assignments, OrderAssignments with their related entities) are projected
    /// at the database level to minimize data transfer and avoid N+1 queries.
    /// </summary>
    public async Task<(IEnumerable<VehicleDto> Items, int TotalCount)> GetAllPaginatedAsDtoAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var paginationParams = new PaginationParams(pageNumber, pageSize);

        // Count total records matching the filter
        var totalCount = await _context
            .Vehicles.AsNoTracking()
            .Where(v => v.TenantId == tenantId)
            .CountAsync(ct);

        // Retrieve paginated data with all required relationships for projection,
        // then project to DTOs at database level for optimal performance
        var items = await _context
            .Vehicles.AsNoTracking()
            .Where(v => v.TenantId == tenantId)
            // Include all required relations for the VehicleDto projection
            .Include(v => v.Tenant)
            .Include(v => v.Assignments)
            .ThenInclude(a => a.Driver)
            .Include(v => v.Assignments)
            .ThenInclude(a => a.Vehicle)
            .ThenInclude(v2 => v2.Tenant)
            .Include(v => v.Assignments)
            .ThenInclude(a => a.Tenant)
            .Include(v => v.OrderAssignments)
            .ThenInclude(oa => oa.Order)
            .Include(v => v.OrderAssignments)
            .ThenInclude(oa => oa.Vehicle)
            .ThenInclude(v2 => v2.Tenant)
            .Include(v => v.OrderAssignments)
            .ThenInclude(oa => oa.Tenant)
            .OrderByDescending(v => v.CreatedAt)
            .Skip(paginationParams.GetSkip())
            .Take(paginationParams.PageSize)
            // Project to DTO at database level using compiled LINQ expressions
            .Select(ProjectionExtensions.ToVehicleDtoExpression)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    /// <summary>
    /// Retrieves a single vehicle as DTO by ID using database-level projection.
    /// All nested relations are projected at database level for performance.
    /// </summary>
    public async Task<VehicleDto?> GetByIdAsDtoAsync(
        Guid id,
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .Vehicles.AsNoTracking()
            .Where(v => v.Id == id && v.TenantId == tenantId)
            // Include all required relations for the VehicleDto projection
            .Include(v => v.Tenant)
            .Include(v => v.Assignments)
            .ThenInclude(a => a.Driver)
            .Include(v => v.Assignments)
            .ThenInclude(a => a.Vehicle)
            .ThenInclude(v2 => v2.Tenant)
            .Include(v => v.Assignments)
            .ThenInclude(a => a.Tenant)
            .Include(v => v.OrderAssignments)
            .ThenInclude(oa => oa.Order)
            .Include(v => v.OrderAssignments)
            .ThenInclude(oa => oa.Vehicle)
            .ThenInclude(v2 => v2.Tenant)
            .Include(v => v.OrderAssignments)
            .ThenInclude(oa => oa.Tenant)
            // Project to DTO at database level using compiled LINQ expressions
            .Select(ProjectionExtensions.ToVehicleDtoExpression)
            .FirstOrDefaultAsync(ct);

    // ==================== Entity Methods (For CRUD Operations) ====================

    // ==================== Entity Methods (For CRUD Operations) ====================

    /// <summary>
    /// Retrieves vehicles with pagination for CRUD operations.
    /// Returns raw entities for mutation (Create, Update, Delete) scenarios.
    /// Includes Tenant, Assignments, and OrderAssignments to prevent lazy-loading on mutation.
    /// For read-only DTOs with projections, use GetAllPaginatedAsDtoAsync() instead.
    /// </summary>
    /// <remarks>
    /// Note: Does not include nested Vehicle, Order, Driver in Assignments since those
    /// are primarily used for DTO projections. CRUD operations typically work
    /// with single-level entity relationships.
    /// </remarks>
    public async Task<(IEnumerable<Vehicle> Items, int TotalCount)> GetAllPaginatedAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var paginationParams = new PaginationParams(pageNumber, pageSize);

        // Count total records matching the tenant filter
        var totalCount = await _context
            .Vehicles.AsNoTracking()
            .Where(v => v.TenantId == tenantId)
            .CountAsync(ct);

        // Retrieve paginated entities with basic relationships for CRUD operations
        var items = await _context
            .Vehicles.AsNoTracking()
            .Where(v => v.TenantId == tenantId)
            // Include only necessary relations for entity mutation scenarios
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
    /// Retrieves all vehicles for a tenant for CRUD operations without pagination.
    /// Use with caution - consider GetAllPaginatedAsync for large datasets.
    /// Returns raw entities with basic relationships.
    /// </summary>
    /// <remarks>
    /// Warning: This method returns the entire result set. For large vehicle collections,
    /// use GetAllPaginatedAsync instead to avoid memory issues and improve performance.
    /// </remarks>
    public async Task<IEnumerable<Vehicle>> GetAllAsync(
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .Vehicles.AsNoTracking()
            .Where(v => v.TenantId == tenantId)
            // Include necessary relations for entity mutation scenarios
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
    /// Retrieves a single vehicle by ID for CRUD operations.
    /// Returns raw entity with basic relationships, ready for mutation.
    /// Enforces tenant isolation at the database query level.
    /// </summary>
    /// <remarks>
    /// This method ensures tenant isolation by filtering both ID and TenantId.
    /// Never rely on TenantId from the caller for security; always validate at query level.
    /// </remarks>
    public async Task<Vehicle?> GetByIdAsync(
        Guid id,
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .Vehicles.AsNoTracking()
            .Where(v => v.Id == id && v.TenantId == tenantId)
            // Include necessary relations for entity mutation scenarios
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

    /// <summary>
    /// Creates a new vehicle entity in the database.
    /// Assumes domain validation has already been performed by the domain aggregate root.
    /// </summary>
    public async Task<Vehicle> CreateAsync(Vehicle vehicle, CancellationToken ct = default)
    {
        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync(ct);
        return vehicle;
    }

    /// <summary>
    /// Updates an existing vehicle entity in the database.
    /// Assumes domain validation has already been performed by the domain aggregate root.
    /// </summary>
    public async Task UpdateAsync(Vehicle vehicle, CancellationToken ct = default)
    {
        _context.Vehicles.Update(vehicle);
        await _context.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Deletes a vehicle from the database.
    /// Enforces tenant isolation to prevent cross-tenant data deletion.
    /// </summary>
    /// <remarks>
    /// Validates tenant ID to ensure the vehicle belongs to the specified tenant
    /// before deletion, protecting against authorization bypasses.
    /// </remarks>
    public async Task DeleteAsync(Guid id, Guid tenantId, CancellationToken ct = default)
    {
        // Query with tenant isolation to prevent deletion of vehicles from other tenants
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
