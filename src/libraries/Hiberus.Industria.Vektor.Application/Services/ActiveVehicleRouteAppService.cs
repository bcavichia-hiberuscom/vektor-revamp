using ErrorOr;
using Hiberus.Industria.Vektor.Application.Common.Mappings;
using Hiberus.Industria.Vektor.Application.DTOs.Route;
using Hiberus.Industria.Vektor.Application.DTOs.Tenant;
using Hiberus.Industria.Vektor.Application.DTOs.Vehicle;
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

    /// <summary>
    /// Retrieves all active vehicle routes for a tenant as DTOs with nested relations.
    /// </summary>
    public async Task<IEnumerable<ActiveVehicleRouteDto>> GetAllAsDto(
        Guid tenantId,
        CancellationToken ct
    )
    {
        var routes = await _routeRepo.GetAllAsync(tenantId, ct);

        return routes
            .Select(r => new ActiveVehicleRouteDto(
                r.Id,
                r.TenantId,
                r.VehicleId,
                r.RoutePayload,
                r.AssociatedOrderIds,
                r.StartedAt,
                r.CreatedAt,
                new VehicleSummaryDto(
                    r.Vehicle.Id,
                    r.Vehicle.Label,
                    r.Vehicle.LicensePlate ?? string.Empty,
                    r.Vehicle.Brand ?? string.Empty,
                    r.Vehicle.Model ?? string.Empty,
                    r.Vehicle.Year ?? 0,
                    r.Vehicle.Type,
                    r.Vehicle.Status.ToString()
                ),
                new TenantSummaryDto(r.Tenant.Id, r.Tenant.Name, r.Tenant.Slug)
            ))
            .ToList();
    }

    /// <summary>
    /// Retrieves all active vehicle routes for a vehicle as DTOs with nested relations.
    /// </summary>
    public async Task<IEnumerable<ActiveVehicleRouteDto>> GetByVehicleAsDto(
        Guid vehicleId,
        Guid tenantId,
        CancellationToken ct
    )
    {
        var routes = await _routeRepo.GetByVehicleAsync(vehicleId, tenantId, ct);

        return routes
            .Select(r => new ActiveVehicleRouteDto(
                r.Id,
                r.TenantId,
                r.VehicleId,
                r.RoutePayload,
                r.AssociatedOrderIds,
                r.StartedAt,
                r.CreatedAt,
                new VehicleSummaryDto(
                    r.Vehicle.Id,
                    r.Vehicle.Label,
                    r.Vehicle.LicensePlate ?? string.Empty,
                    r.Vehicle.Brand ?? string.Empty,
                    r.Vehicle.Model ?? string.Empty,
                    r.Vehicle.Year ?? 0,
                    r.Vehicle.Type,
                    r.Vehicle.Status.ToString()
                ),
                new TenantSummaryDto(r.Tenant.Id, r.Tenant.Name, r.Tenant.Slug)
            ))
            .ToList();
    }

    /// <summary>
    /// Retrieves a single active vehicle route by ID as DTO with nested relations.
    /// </summary>
    public async Task<ActiveVehicleRouteDto?> GetByIdAsDto(
        Guid id,
        Guid tenantId,
        CancellationToken ct
    )
    {
        var route = await _routeRepo.GetByIdAsync(id, tenantId, ct);
        if (route is null)
            return null;

        return new ActiveVehicleRouteDto(
            route.Id,
            route.TenantId,
            route.VehicleId,
            route.RoutePayload,
            route.AssociatedOrderIds,
            route.StartedAt,
            route.CreatedAt,
            new VehicleSummaryDto(
                route.Vehicle.Id,
                route.Vehicle.Label,
                route.Vehicle.LicensePlate ?? string.Empty,
                route.Vehicle.Brand ?? string.Empty,
                route.Vehicle.Model ?? string.Empty,
                route.Vehicle.Year ?? 0,
                route.Vehicle.Type,
                route.Vehicle.Status.ToString()
            ),
            new TenantSummaryDto(route.Tenant.Id, route.Tenant.Name, route.Tenant.Slug)
        );
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

    /// <summary>
    /// Retrieves a route history by ID as DTO with nested relations.
    /// Used after Complete() to return full RouteHistoryDto with all nested data.
    /// </summary>
    public async Task<RouteHistoryDto?> GetHistoryByIdAsDto(
        Guid historyId,
        Guid tenantId,
        CancellationToken ct
    )
    {
        var history = await _historyRepo.GetByIdAsync(historyId, tenantId, ct);
        if (history is null)
            return null;

        return new RouteHistoryDto(
            history.Id,
            history.TenantId,
            history.VehicleId,
            history.RoutePayload,
            history.AssociatedOrderIds,
            history.StartedAt,
            history.FinishedAt,
            history.FinishedAt - history.StartedAt,
            new VehicleSummaryDto(
                history.Vehicle.Id,
                history.Vehicle.Label,
                history.Vehicle.LicensePlate ?? string.Empty,
                history.Vehicle.Brand ?? string.Empty,
                history.Vehicle.Model ?? string.Empty,
                history.Vehicle.Year ?? 0,
                history.Vehicle.Type,
                history.Vehicle.Status.ToString()
            ),
            new TenantSummaryDto(history.Tenant.Id, history.Tenant.Name, history.Tenant.Slug)
        );
    }
}
