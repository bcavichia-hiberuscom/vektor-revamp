using ErrorOr;
using Hiberus.Industria.Vektor.Application.DTOs;
using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.ActiveVehicleRoute;
using Hiberus.Industria.Vektor.Domain.RouteHistory;

public class ActiveVehicleRouteAppService
{
    private readonly IActiveVehicleRouteRepository _routeRepo;
    private readonly IRouteHistoryRepository _historyRepo;

    public ActiveVehicleRouteAppService(
        IActiveVehicleRouteRepository routeRepo,
        IRouteHistoryRepository historyRepo
    )
    {
        _routeRepo = routeRepo;
        _historyRepo = historyRepo;
    }

    public async Task<IEnumerable<ActiveVehicleRoute>> GetAll(
        Guid tenantId,
        CancellationToken ct
    ) => await _routeRepo.GetAllAsync(tenantId, ct);

    public async Task<IEnumerable<ActiveVehicleRoute>> GetByVehicle(
        Guid vehicleId,
        Guid tenantId,
        CancellationToken ct
    ) => await _routeRepo.GetByVehicleAsync(vehicleId, tenantId, ct);

    public async Task<ActiveVehicleRoute?> GetById(Guid id, Guid tenantId, CancellationToken ct) =>
        await _routeRepo.GetByIdAsync(id, tenantId, ct);

    public async Task<ErrorOr<ActiveVehicleRoute>> Create(
        CreateActiveVehicleRouteDto dto,
        Guid tenantId,
        CancellationToken ct
    )
    {
        var result = ActiveVehicleRoute.Create(
            tenantId,
            dto.VehicleId,
            dto.RoutePayload,
            dto.AssociatedOrderIds,
            "system" // TODO: replace with authenticated user
        );

        if (result.IsError)
            return result.Errors;

        return await _routeRepo.CreateAsync(result.Value, ct);
    }

    public async Task<ErrorOr<ActiveVehicleRoute>> Update(
        Guid id,
        Guid tenantId,
        UpdateActiveVehicleRouteDto dto,
        CancellationToken ct
    )
    {
        var route = await _routeRepo.GetByIdAsync(id, tenantId, ct);
        if (route is null)
            return Error.NotFound("ActiveVehicleRoute.NotFound", "Route not found");

        var result = route.UpdateRoute(
            dto.RoutePayload,
            dto.AssociatedOrderIds,
            "system" // TODO: replace with authenticated user
        );

        if (result.IsError)
            return result.Errors;

        await _routeRepo.UpdateAsync(route, ct);
        return route;
    }

    // Completes the route: creates the history and deletes the active route in a single operation
    public async Task<ErrorOr<RouteHistory>> Complete(Guid id, Guid tenantId, CancellationToken ct)
    {
        var route = await _routeRepo.GetByIdAsync(id, tenantId, ct);
        if (route is null)
            return Error.NotFound("ActiveVehicleRoute.NotFound", "Route not found");

        // The domain constructs the history from the active route
        var historyResult = route.Complete("system"); // TODO: replace with authenticated user
        if (historyResult.IsError)
            return historyResult.Errors;

        // Transaction: save history + delete active route
        var history = await _historyRepo.CreateAsync(historyResult.Value, ct);
        await _routeRepo.DeleteAsync(route, ct);

        return history;
    }
}
