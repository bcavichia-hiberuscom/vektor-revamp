using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.RouteHistory;

public class RouteHistoryAppService
{
    private readonly IRouteHistoryRepository _repo;

    public RouteHistoryAppService(IRouteHistoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<RouteHistory>> GetByTenant(Guid tenantId, CancellationToken ct) =>
        await _repo.GetByTenantAsync(tenantId, ct);

    public async Task<IEnumerable<RouteHistory>> GetByVehicle(
        Guid vehicleId,
        Guid tenantId,
        CancellationToken ct
    ) => await _repo.GetByVehicleAsync(vehicleId, tenantId, ct);

    public async Task<RouteHistory?> GetById(Guid id, Guid tenantId, CancellationToken ct) =>
        await _repo.GetByIdAsync(id, tenantId, ct);
}
