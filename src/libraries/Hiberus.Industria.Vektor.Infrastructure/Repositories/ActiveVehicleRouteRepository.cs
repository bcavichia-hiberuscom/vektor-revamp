using Hiberus.Industria.Vektor.Application.Common.Mappings;
using Hiberus.Industria.Vektor.Application.Common.Pagination;
using Hiberus.Industria.Vektor.Application.DTOs.Route;
using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.ActiveVehicleRoute;
using Hiberus.Industria.Vektor.Domain.RouteHistory;
using Hiberus.Industria.Vektor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hiberus.Industria.Vektor.Infrastructure.Repositories;

public class ActiveVehicleRouteRepository : IActiveVehicleRouteRepository
{
    private readonly VektorDbContext _context;

    public ActiveVehicleRouteRepository(VektorDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves active vehicle routes with pagination, eager-loading related Vehicle and Tenant.
    /// Uses AsNoTracking() for read-only queries to improve performance.
    /// </summary>
    public async Task<(IEnumerable<ActiveVehicleRoute> Items, int TotalCount)> GetAllPaginatedAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var paginationParams = new PaginationParams(pageNumber, pageSize);

        // Get total count without loading data
        var totalCount = await _context
            .ActiveVehicleRoutes.AsNoTracking()
            .Where(r => r.TenantId == tenantId)
            .CountAsync(ct);

        // Get paginated data with eager-loaded relations
        var items = await _context
            .ActiveVehicleRoutes.AsNoTracking()
            .Where(r => r.TenantId == tenantId)
            .Include(r => r.Vehicle)
            .Include(r => r.Tenant)
            .OrderByDescending(r => r.CreatedAt)
            .Skip(paginationParams.GetSkip())
            .Take(paginationParams.PageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    /// <summary>
    /// Retrieves all active vehicle routes for a tenant with eager-loaded relations.
    /// Use with caution - consider GetAllPaginatedAsync for large datasets.
    /// </summary>
    public async Task<IEnumerable<ActiveVehicleRoute>> GetAllAsync(
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .ActiveVehicleRoutes.AsNoTracking()
            .Where(r => r.TenantId == tenantId)
            .Include(r => r.Vehicle)
            .Include(r => r.Tenant)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(ct);

    /// <summary>
    /// Retrieves active routes for a specific vehicle with eager-loaded relations.
    /// </summary>
    public async Task<IEnumerable<ActiveVehicleRoute>> GetByVehicleAsync(
        Guid vehicleId,
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .ActiveVehicleRoutes.AsNoTracking()
            .Where(r => r.VehicleId == vehicleId && r.TenantId == tenantId)
            .Include(r => r.Vehicle)
            .Include(r => r.Tenant)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(ct);

    /// <summary>
    /// Retrieves a single active vehicle route by ID with eager-loaded relations.
    /// </summary>
    public async Task<ActiveVehicleRoute?> GetByIdAsync(
        Guid id,
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .ActiveVehicleRoutes.AsNoTracking()
            .Where(r => r.Id == id && r.TenantId == tenantId)
            .Include(r => r.Vehicle)
            .Include(r => r.Tenant)
            .FirstOrDefaultAsync(ct);

    /// <summary>
    /// Retrieves active vehicle routes with pagination, returning DTOs with nested relations.
    /// Uses database-level projection to avoid N+1 queries.
    /// </summary>
    public async Task<(
        IEnumerable<ActiveVehicleRouteDto> Items,
        int TotalCount
    )> GetAllPaginatedAsDtoAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var paginationParams = new PaginationParams(pageNumber, pageSize);

        // Get total count
        var totalCount = await _context
            .ActiveVehicleRoutes.AsNoTracking()
            .Where(r => r.TenantId == tenantId)
            .CountAsync(ct);

        // Get paginated DTOs via projection
        var dtos = await _context
            .ActiveVehicleRoutes.AsNoTracking()
            .Where(r => r.TenantId == tenantId)
            .Include(r => r.Vehicle)
            .ThenInclude(v => v.Tenant)
            .Include(r => r.Tenant)
            .OrderByDescending(r => r.CreatedAt)
            .Skip(paginationParams.GetSkip())
            .Take(paginationParams.PageSize)
            .Select(ProjectionExtensions.ToActiveVehicleRouteDtoExpression)
            .ToListAsync(ct);

        return (dtos, totalCount);
    }

    /// <summary>
    /// Retrieves a single active vehicle route by ID, returning DTO with nested relations.
    /// Uses database-level projection for optimal performance.
    /// </summary>
    public async Task<ActiveVehicleRouteDto?> GetByIdAsDtoAsync(
        Guid id,
        Guid tenantId,
        CancellationToken ct
    ) =>
        await _context
            .ActiveVehicleRoutes.AsNoTracking()
            .Where(r => r.Id == id && r.TenantId == tenantId)
            .Include(r => r.Vehicle)
            .ThenInclude(v => v.Tenant)
            .Include(r => r.Tenant)
            .Select(ProjectionExtensions.ToActiveVehicleRouteDtoExpression)
            .FirstOrDefaultAsync(ct);

    public async Task<ActiveVehicleRoute> CreateAsync(
        ActiveVehicleRoute route,
        CancellationToken ct = default
    )
    {
        _context.ActiveVehicleRoutes.Add(route);
        await _context.SaveChangesAsync(ct);
        return route;
    }

    public async Task UpdateAsync(ActiveVehicleRoute route, CancellationToken ct = default)
    {
        _context.ActiveVehicleRoutes.Update(route);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(ActiveVehicleRoute route, CancellationToken ct = default)
    {
        _context.ActiveVehicleRoutes.Remove(route);
        await _context.SaveChangesAsync(ct);
    }
}

public class RouteHistoryRepository : IRouteHistoryRepository
{
    private readonly VektorDbContext _context;

    public RouteHistoryRepository(VektorDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves route histories with pagination for a tenant.
    /// Uses AsNoTracking() for read-only queries to improve performance.
    /// </summary>
    public async Task<(IEnumerable<RouteHistory> Items, int TotalCount)> GetAllPaginatedAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var paginationParams = new PaginationParams(pageNumber, pageSize);

        // Get total count without loading data
        var totalCount = await _context
            .RouteHistories.AsNoTracking()
            .Where(h => h.TenantId == tenantId)
            .CountAsync(ct);

        // Get paginated data with eager-loaded relations
        var items = await _context
            .RouteHistories.AsNoTracking()
            .Where(h => h.TenantId == tenantId)
            .OrderByDescending(h => h.FinishedAt)
            .Skip(paginationParams.GetSkip())
            .Take(paginationParams.PageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    public async Task<IEnumerable<RouteHistory>> GetByTenantAsync(
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .RouteHistories.AsNoTracking()
            .Where(h => h.TenantId == tenantId)
            .OrderByDescending(h => h.FinishedAt)
            .ToListAsync(ct);

    public async Task<IEnumerable<RouteHistory>> GetByVehicleAsync(
        Guid vehicleId,
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .RouteHistories.AsNoTracking()
            .Where(h => h.VehicleId == vehicleId && h.TenantId == tenantId)
            .OrderByDescending(h => h.FinishedAt)
            .ToListAsync(ct);

    public async Task<RouteHistory?> GetByIdAsync(
        Guid id,
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .RouteHistories.AsNoTracking()
            .FirstOrDefaultAsync(h => h.Id == id && h.TenantId == tenantId, ct);

    /// <summary>
    /// Retrieves route history records with pagination, returning DTOs with nested relations.
    /// Uses database-level projection to avoid N+1 queries.
    /// </summary>
    public async Task<(
        IEnumerable<RouteHistoryDto> Items,
        int TotalCount
    )> GetAllPaginatedAsDtoAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var paginationParams = new PaginationParams(pageNumber, pageSize);

        // Get total count
        var totalCount = await _context
            .RouteHistories.AsNoTracking()
            .Where(h => h.TenantId == tenantId)
            .CountAsync(ct);

        // Get paginated DTOs via projection
        var dtos = await _context
            .RouteHistories.AsNoTracking()
            .Where(h => h.TenantId == tenantId)
            .Include(h => h.Vehicle)
            .ThenInclude(v => v.Tenant)
            .Include(h => h.Tenant)
            .OrderByDescending(h => h.CreatedAt)
            .Skip(paginationParams.GetSkip())
            .Take(paginationParams.PageSize)
            .Select(ProjectionExtensions.ToRouteHistoryDtoExpression)
            .ToListAsync(ct);

        return (dtos, totalCount);
    }

    /// <summary>
    /// Retrieves a single route history record by ID, returning DTO with nested relations.
    /// Uses database-level projection for optimal performance.
    /// </summary>
    public async Task<RouteHistoryDto?> GetByIdAsDtoAsync(
        Guid id,
        Guid tenantId,
        CancellationToken ct
    ) =>
        await _context
            .RouteHistories.AsNoTracking()
            .Where(h => h.Id == id && h.TenantId == tenantId)
            .Include(h => h.Vehicle)
            .ThenInclude(v => v.Tenant)
            .Include(h => h.Tenant)
            .Select(ProjectionExtensions.ToRouteHistoryDtoExpression)
            .FirstOrDefaultAsync(ct);

    public async Task<RouteHistory> CreateAsync(
        RouteHistory history,
        CancellationToken ct = default
    )
    {
        _context.RouteHistories.Add(history);
        await _context.SaveChangesAsync(ct);
        return history;
    }
}
