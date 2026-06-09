using Hiberus.Industria.Vektor.Domain.ActiveVehicleRoute;
using Hiberus.Industria.Vektor.Domain.RouteHistory;

namespace Hiberus.Industria.Vektor.Application.Interfaces;

public interface IActiveVehicleRouteRepository
{
    Task<IEnumerable<ActiveVehicleRoute>> GetAllAsync(
        Guid tenantId,
        CancellationToken ct = default
    );
    Task<IEnumerable<ActiveVehicleRoute>> GetByVehicleAsync(
        Guid vehicleId,
        Guid tenantId,
        CancellationToken ct = default
    );
    Task<ActiveVehicleRoute?> GetByIdAsync(Guid id, Guid tenantId, CancellationToken ct = default);
    Task<ActiveVehicleRoute> CreateAsync(ActiveVehicleRoute route, CancellationToken ct = default);
    Task UpdateAsync(ActiveVehicleRoute route, CancellationToken ct = default);
    Task DeleteAsync(ActiveVehicleRoute route, CancellationToken ct = default); // hard delete: la ruta se completó
}

public interface IRouteHistoryRepository
{
    Task<IEnumerable<RouteHistory>> GetByTenantAsync(Guid tenantId, CancellationToken ct = default);
    Task<IEnumerable<RouteHistory>> GetByVehicleAsync(
        Guid vehicleId,
        Guid tenantId,
        CancellationToken ct = default
    );
    Task<RouteHistory?> GetByIdAsync(Guid id, Guid tenantId, CancellationToken ct = default);
    Task<RouteHistory> CreateAsync(RouteHistory history, CancellationToken ct = default);
}
