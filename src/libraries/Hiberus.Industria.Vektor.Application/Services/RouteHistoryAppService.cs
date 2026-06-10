using Hiberus.Industria.Vektor.Application.Common.Mappings;
using Hiberus.Industria.Vektor.Application.DTOs.Route;
using Hiberus.Industria.Vektor.Application.DTOs.Vehicle;
using Hiberus.Industria.Vektor.Application.DTOs.Tenant;
using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.RouteHistory;

public class RouteHistoryAppService
{
    private readonly IRouteHistoryRepository _repo;

    public RouteHistoryAppService(IRouteHistoryRepository repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// Retrieves all route histories for a tenant as DTOs with nested relations.
    /// </summary>
    public async Task<IEnumerable<RouteHistoryDto>> GetByTenantAsDto(Guid tenantId, CancellationToken ct)
    {
        var histories = await _repo.GetByTenantAsync(tenantId, ct);
        
        return histories
            .Select(h => new RouteHistoryDto(
                h.Id,
                h.TenantId,
                h.VehicleId,
                h.RoutePayload,
                h.AssociatedOrderIds,
                h.StartedAt,
                h.FinishedAt,
                h.FinishedAt - h.StartedAt,
                new VehicleSummaryDto(
                    h.Vehicle.Id,
                    h.Vehicle.Label,
                    h.Vehicle.LicensePlate ?? string.Empty,
                    h.Vehicle.Brand ?? string.Empty,
                    h.Vehicle.Model ?? string.Empty,
                    h.Vehicle.Year ?? 0,
                    h.Vehicle.Type,
                    h.Vehicle.Status.ToString()
                ),
                new TenantSummaryDto(h.Tenant.Id, h.Tenant.Name, h.Tenant.Slug)
            ))
            .ToList();
    }

    /// <summary>
    /// Retrieves all route histories for a vehicle as DTOs with nested relations.
    /// </summary>
    public async Task<IEnumerable<RouteHistoryDto>> GetByVehicleAsDto(
        Guid vehicleId,
        Guid tenantId,
        CancellationToken ct
    )
    {
        var histories = await _repo.GetByVehicleAsync(vehicleId, tenantId, ct);
        
        return histories
            .Select(h => new RouteHistoryDto(
                h.Id,
                h.TenantId,
                h.VehicleId,
                h.RoutePayload,
                h.AssociatedOrderIds,
                h.StartedAt,
                h.FinishedAt,
                h.FinishedAt - h.StartedAt,
                new VehicleSummaryDto(
                    h.Vehicle.Id,
                    h.Vehicle.Label,
                    h.Vehicle.LicensePlate ?? string.Empty,
                    h.Vehicle.Brand ?? string.Empty,
                    h.Vehicle.Model ?? string.Empty,
                    h.Vehicle.Year ?? 0,
                    h.Vehicle.Type,
                    h.Vehicle.Status.ToString()
                ),
                new TenantSummaryDto(h.Tenant.Id, h.Tenant.Name, h.Tenant.Slug)
            ))
            .ToList();
    }

    /// <summary>
    /// Retrieves a single route history by ID as DTO with nested relations.
    /// </summary>
    public async Task<RouteHistoryDto?> GetByIdAsDto(Guid id, Guid tenantId, CancellationToken ct)
    {
        var history = await _repo.GetByIdAsync(id, tenantId, ct);
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
