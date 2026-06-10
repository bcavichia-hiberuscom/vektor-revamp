using Hiberus.Industria.Vektor.Application.DTOs.Route;
using Hiberus.Industria.Vektor.Domain.ActiveVehicleRoute;
using Hiberus.Industria.Vektor.Domain.RouteHistory;

namespace Hiberus.Industria.Vektor.Application.Interfaces;

public interface IActiveVehicleRouteRepository
{
    /// <summary>
    /// Retrieves all active vehicle routes for a given tenant with pagination support.
    /// </summary>
    /// <returns>Tuple containing the collection of routes and total count.</returns>
    Task<(IEnumerable<ActiveVehicleRoute> Items, int TotalCount)> GetAllPaginatedAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    );

    /// <summary>
    /// Retrieves all active vehicle routes for a given tenant as DTOs with pagination support.
    /// Uses database-level projection to minimize data transfer.
    /// </summary>
    /// <returns>Tuple containing the collection of ActiveVehicleRouteDto and total count.</returns>
    Task<(IEnumerable<ActiveVehicleRouteDto> Items, int TotalCount)> GetAllPaginatedAsDtoAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    );

    /// <summary>
    /// Retrieves a single active vehicle route by ID as DTO with eager-loaded relations.
    /// Uses database-level projection for optimal performance.
    /// </summary>
    Task<ActiveVehicleRouteDto?> GetByIdAsDtoAsync(
        Guid id,
        Guid tenantId,
        CancellationToken ct = default
    );

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
    /// <summary>
    /// Retrieves all route history records for a given tenant with pagination support.
    /// </summary>
    /// <returns>Tuple containing the collection of records and total count.</returns>
    Task<(IEnumerable<RouteHistory> Items, int TotalCount)> GetAllPaginatedAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    );

    /// <summary>
    /// Retrieves all route history records for a given tenant as DTOs with pagination support.
    /// Uses database-level projection to minimize data transfer.
    /// </summary>
    /// <returns>Tuple containing the collection of RouteHistoryDto and total count.</returns>
    Task<(IEnumerable<RouteHistoryDto> Items, int TotalCount)> GetAllPaginatedAsDtoAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    );

    /// <summary>
    /// Retrieves a single route history record by ID as DTO with eager-loaded relations.
    /// Uses database-level projection for optimal performance.
    /// </summary>
    Task<RouteHistoryDto?> GetByIdAsDtoAsync(
        Guid id,
        Guid tenantId,
        CancellationToken ct = default
    );

    Task<IEnumerable<RouteHistory>> GetByTenantAsync(Guid tenantId, CancellationToken ct = default);
    Task<IEnumerable<RouteHistory>> GetByVehicleAsync(
        Guid vehicleId,
        Guid tenantId,
        CancellationToken ct = default
    );
    Task<RouteHistory?> GetByIdAsync(Guid id, Guid tenantId, CancellationToken ct = default);
    Task<RouteHistory> CreateAsync(RouteHistory history, CancellationToken ct = default);
}
