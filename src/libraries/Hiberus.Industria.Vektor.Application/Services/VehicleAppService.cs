using ErrorOr;
using Hiberus.Industria.Vektor.Application.Common.Mappings;
using Hiberus.Industria.Vektor.Application.Common.Pagination;
using Hiberus.Industria.Vektor.Application.DTOs.Vehicle;
using Hiberus.Industria.Vektor.Application.DTOs.DriverVehicleAssignment;
using Hiberus.Industria.Vektor.Application.DTOs.Driver;
using Hiberus.Industria.Vektor.Application.DTOs.OrderAssignment;
using Hiberus.Industria.Vektor.Application.DTOs.Order;
using Hiberus.Industria.Vektor.Application.DTOs.Tenant;
using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.Vehicle;

public class VehicleAppService
{
    private readonly IVehicleRepository _repo;

    public VehicleAppService(IVehicleRepository repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// Retrieves vehicles with pagination, returning DTOs with nested relations.
    /// Default: 20 items/page, maximum: 100 items/page.
    /// </summary>
    public async Task<PagedResult<VehicleDto>> GetAllPaginatedAsync(
        Guid tenantId,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken ct = default
    )
    {
        var (vehicles, totalCount) = await _repo.GetAllPaginatedAsync(tenantId, pageNumber, pageSize, ct);
        
        // Project entities to DTOs with nested relations
        var dtos = vehicles
            .Select(v => new VehicleDto(
                v.Id,
                v.TenantId,
                v.Label,
                v.LicensePlate,
                v.Brand,
                v.Model,
                v.Year,
                v.Type,
                v.Status.ToString(),
                new TenantSummaryDto(v.Tenant.Id, v.Tenant.Name, v.Tenant.Slug),
                v.Assignments
                    .Select(a => new DriverVehicleAssignmentDto(
                        a.Id,
                        a.TenantId,
                        a.DriverId,
                        a.VehicleId,
                        a.AssignedAt,
                        a.UnassignedAt,
                        new DriverSummaryDto(
                            a.Driver.Id,
                            a.Driver.Name,
                            a.Driver.PhoneNumber,
                            a.Driver.LicenseType,
                            a.Driver.IsAvailable
                        ),
                        new VehicleSummaryDto(
                            a.Vehicle.Id,
                            a.Vehicle.Label,
                            a.Vehicle.LicensePlate ?? string.Empty,
                            a.Vehicle.Brand ?? string.Empty,
                            a.Vehicle.Model ?? string.Empty,
                            a.Vehicle.Year ?? 0,
                            a.Vehicle.Type,
                            a.Vehicle.Status.ToString()
                        ),
                        new TenantSummaryDto(a.Tenant.Id, a.Tenant.Name, a.Tenant.Slug)
                    ))
                    .ToList(),
                v.OrderAssignments
                    .Select(a => new OrderAssignmentDto(
                        a.Id,
                        a.TenantId,
                        a.OrderId,
                        a.VehicleId,
                        a.Status.ToString(),
                        a.AssignedAt,
                        a.StartedAt,
                        a.CompletedAt,
                        a.ActualArrival,
                        a.FailureReason,
                        a.CreatedAt,
                        a.UpdatedAt,
                        new OrderSummaryDto(a.Order.Id, a.Order.Label, a.Order.Status.ToString(), a.Order.CustomerName),
                        new VehicleSummaryDto(
                            a.Vehicle.Id,
                            a.Vehicle.Label,
                            a.Vehicle.LicensePlate ?? string.Empty,
                            a.Vehicle.Brand ?? string.Empty,
                            a.Vehicle.Model ?? string.Empty,
                            a.Vehicle.Year ?? 0,
                            a.Vehicle.Type,
                            a.Vehicle.Status.ToString()
                        ),
                        new TenantSummaryDto(a.Tenant.Id, a.Tenant.Name, a.Tenant.Slug)
                    ))
                    .ToList()
            ))
            .ToList();

        return new PagedResult<VehicleDto>(dtos, totalCount, pageNumber, pageSize);
    }

    /// <summary>
    /// Retrieves a single vehicle by ID as DTO with nested relations.
    /// </summary>
    public async Task<VehicleDto?> GetByIdAsDto(Guid id, Guid tenantId, CancellationToken ct)
    {
        var vehicle = await _repo.GetByIdAsync(id, tenantId, ct);
        if (vehicle is null)
            return null;

        return new VehicleDto(
            vehicle.Id,
            vehicle.TenantId,
            vehicle.Label,
            vehicle.LicensePlate,
            vehicle.Brand,
            vehicle.Model,
            vehicle.Year,
            vehicle.Type,
            vehicle.Status.ToString(),
            new TenantSummaryDto(vehicle.Tenant.Id, vehicle.Tenant.Name, vehicle.Tenant.Slug),
            vehicle.Assignments
                .Select(a => new DriverVehicleAssignmentDto(
                    a.Id,
                    a.TenantId,
                    a.DriverId,
                    a.VehicleId,
                    a.AssignedAt,
                    a.UnassignedAt,
                    new DriverSummaryDto(
                        a.Driver.Id,
                        a.Driver.Name,
                        a.Driver.PhoneNumber,
                        a.Driver.LicenseType,
                        a.Driver.IsAvailable
                    ),
                    new VehicleSummaryDto(
                        a.Vehicle.Id,
                        a.Vehicle.Label,
                        a.Vehicle.LicensePlate ?? string.Empty,
                        a.Vehicle.Brand ?? string.Empty,
                        a.Vehicle.Model ?? string.Empty,
                        a.Vehicle.Year ?? 0,
                        a.Vehicle.Type,
                        a.Vehicle.Status.ToString()
                    ),
                    new TenantSummaryDto(a.Tenant.Id, a.Tenant.Name, a.Tenant.Slug)
                ))
                .ToList(),
            vehicle.OrderAssignments
                .Select(a => new OrderAssignmentDto(
                    a.Id,
                    a.TenantId,
                    a.OrderId,
                    a.VehicleId,
                    a.Status.ToString(),
                    a.AssignedAt,
                    a.StartedAt,
                    a.CompletedAt,
                    a.ActualArrival,
                    a.FailureReason,
                    a.CreatedAt,
                    a.UpdatedAt,
                    new OrderSummaryDto(a.Order.Id, a.Order.Label, a.Order.Status.ToString(), a.Order.CustomerName),
                    new VehicleSummaryDto(
                        a.Vehicle.Id,
                        a.Vehicle.Label,
                        a.Vehicle.LicensePlate ?? string.Empty,
                        a.Vehicle.Brand ?? string.Empty,
                        a.Vehicle.Model ?? string.Empty,
                        a.Vehicle.Year ?? 0,
                        a.Vehicle.Type,
                        a.Vehicle.Status.ToString()
                    ),
                    new TenantSummaryDto(a.Tenant.Id, a.Tenant.Name, a.Tenant.Slug)
                ))
                .ToList()
        );
    }

    public async Task<Vehicle?> GetById(Guid id, Guid tenantId, CancellationToken ct) =>
        await _repo.GetByIdAsync(id, tenantId, ct);

    public async Task<ErrorOr<Vehicle>> Create(
        CreateVehicleDto dto,
        Guid tenantId,
        CancellationToken ct
    )
    {
        var result = Vehicle.Create(
            tenantId,
            dto.Label,
            dto.Type,
            "system", //replace with user when auth is implemented
            dto.LicensePlate,
            dto.Brand,
            dto.Model,
            dto.Year
        );

        if (result.IsError)
            return result.Errors;

        var vehicle = await _repo.CreateAsync(result.Value, ct);
        return vehicle;
    }

    public async Task<ErrorOr<Vehicle>> Update(
        Guid id,
        Guid tenantId,
        UpdateVehicleDto dto,
        CancellationToken ct
    )
    {
        var vehicle = await _repo.GetByIdAsync(id, tenantId, ct);
        if (vehicle is null)
            return Error.NotFound();

        var result = vehicle.Update(
            dto.Label,
            dto.LicensePlate,
            dto.Brand,
            dto.Model,
            dto.Year,
            "system"
        );

        if (result.IsError)
            return result.Errors;

        await _repo.UpdateAsync(vehicle, ct);
        return vehicle;
    }

    public async Task Delete(Guid id, Guid tenantId, CancellationToken ct) =>
        await _repo.DeleteAsync(id, tenantId, ct);
}
