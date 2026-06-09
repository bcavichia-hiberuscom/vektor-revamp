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

    public async Task<IEnumerable<ActiveVehicleRoute>> GetAllAsync(
        Guid tenantId,
        CancellationToken ct = default
    ) => await _context.ActiveVehicleRoutes.Where(r => r.TenantId == tenantId).ToListAsync(ct);

    public async Task<IEnumerable<ActiveVehicleRoute>> GetByVehicleAsync(
        Guid vehicleId,
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .ActiveVehicleRoutes.Where(r => r.VehicleId == vehicleId && r.TenantId == tenantId)
            .ToListAsync(ct);

    public async Task<ActiveVehicleRoute?> GetByIdAsync(
        Guid id,
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context.ActiveVehicleRoutes.FirstOrDefaultAsync(
            r => r.Id == id && r.TenantId == tenantId,
            ct
        );

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

    public async Task<IEnumerable<RouteHistory>> GetByTenantAsync(
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .RouteHistories.Where(h => h.TenantId == tenantId)
            .OrderByDescending(h => h.FinishedAt)
            .ToListAsync(ct);

    public async Task<IEnumerable<RouteHistory>> GetByVehicleAsync(
        Guid vehicleId,
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .RouteHistories.Where(h => h.VehicleId == vehicleId && h.TenantId == tenantId)
            .OrderByDescending(h => h.FinishedAt)
            .ToListAsync(ct);

    public async Task<RouteHistory?> GetByIdAsync(
        Guid id,
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context.RouteHistories.FirstOrDefaultAsync(
            h => h.Id == id && h.TenantId == tenantId,
            ct
        );

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
