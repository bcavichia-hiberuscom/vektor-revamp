using ErrorOr;
using Hiberus.Industria.Vektor.Application.DTOs.Driver;
using Hiberus.Industria.Vektor.Application.DTOs.DriverVehicleAssignment;
using Hiberus.Industria.Vektor.Application.DTOs.Tenant;
using Hiberus.Industria.Vektor.Application.DTOs.Vehicle;
using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.DriverVehicleAssignment;

public class DriverVehicleAssignmentAppService
{
    private readonly IDriverVehicleAssignmentRepository _repo;

    public DriverVehicleAssignmentAppService(IDriverVehicleAssignmentRepository repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// Retrieves all driver-vehicle assignments for a tenant as DTOs with nested relations.
    /// </summary>
    public async Task<IEnumerable<DriverVehicleAssignmentDto>> GetAllAsDto(
        Guid tenantId,
        CancellationToken ct
    )
    {
        var assignments = await _repo.GetByTenantAsync(tenantId, ct);

        return assignments
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
            .ToList();
    }

    /// <summary>
    /// Retrieves all driver-vehicle assignments for a driver as DTOs with nested relations.
    /// </summary>
    public async Task<IEnumerable<DriverVehicleAssignmentDto>> GetByDriverAsDto(
        Guid driverId,
        Guid tenantId,
        CancellationToken ct
    )
    {
        var assignments = await _repo.GetByDriverAsync(driverId, tenantId, ct);

        return assignments
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
            .ToList();
    }

    /// <summary>
    /// Retrieves all driver-vehicle assignments for a vehicle as DTOs with nested relations.
    /// </summary>
    public async Task<IEnumerable<DriverVehicleAssignmentDto>> GetByVehicleAsDto(
        Guid vehicleId,
        Guid tenantId,
        CancellationToken ct
    )
    {
        var assignments = await _repo.GetByVehicleAsync(vehicleId, tenantId, ct);

        return assignments
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
            .ToList();
    }

    /// <summary>
    /// Retrieves a single driver-vehicle assignment by ID as DTO with nested relations.
    /// </summary>
    public async Task<DriverVehicleAssignmentDto?> GetByIdAsDto(
        Guid id,
        Guid tenantId,
        CancellationToken ct
    )
    {
        var assignment = await _repo.GetByIdAsync(id, tenantId, ct);
        if (assignment is null)
            return null;

        return new DriverVehicleAssignmentDto(
            assignment.Id,
            assignment.TenantId,
            assignment.DriverId,
            assignment.VehicleId,
            assignment.AssignedAt,
            assignment.UnassignedAt,
            new DriverSummaryDto(
                assignment.Driver.Id,
                assignment.Driver.Name,
                assignment.Driver.PhoneNumber,
                assignment.Driver.LicenseType,
                assignment.Driver.IsAvailable
            ),
            new VehicleSummaryDto(
                assignment.Vehicle.Id,
                assignment.Vehicle.Label,
                assignment.Vehicle.LicensePlate ?? string.Empty,
                assignment.Vehicle.Brand ?? string.Empty,
                assignment.Vehicle.Model ?? string.Empty,
                assignment.Vehicle.Year ?? 0,
                assignment.Vehicle.Type,
                assignment.Vehicle.Status.ToString()
            ),
            new TenantSummaryDto(
                assignment.Tenant.Id,
                assignment.Tenant.Name,
                assignment.Tenant.Slug
            )
        );
    }

    public async Task<IEnumerable<DriverVehicleAssignment>> GetAll(
        Guid tenantId,
        CancellationToken ct
    ) => await _repo.GetByTenantAsync(tenantId, ct);

    public async Task<DriverVehicleAssignment?> GetById(
        Guid id,
        Guid tenantId,
        CancellationToken ct
    ) => await _repo.GetByIdAsync(id, tenantId, ct);

    public async Task<IEnumerable<DriverVehicleAssignment>> GetByDriver(
        Guid driverId,
        Guid tenantId,
        CancellationToken ct
    ) => await _repo.GetByDriverAsync(driverId, tenantId, ct);

    public async Task<IEnumerable<DriverVehicleAssignment>> GetByVehicle(
        Guid vehicleId,
        Guid tenantId,
        CancellationToken ct
    ) => await _repo.GetByVehicleAsync(vehicleId, tenantId, ct);

    public async Task<ErrorOr<DriverVehicleAssignment>> Create(
        CreateDriverVehicleAssignmentDto dto,
        Guid tenantId,
        CancellationToken ct
    )
    {
        var result = DriverVehicleAssignment.Create(
            tenantId,
            dto.DriverId,
            dto.VehicleId,
            "system" // replace with user when auth is implemented
        );

        if (result.IsError)
            return result.Errors;

        var assignment = await _repo.CreateAsync(result.Value, ct);

        return assignment;
    }

    public async Task<ErrorOr<DriverVehicleAssignment>> Unassign(
        Guid id,
        Guid tenantId,
        CancellationToken ct
    )
    {
        var assignment = await _repo.GetByIdAsync(id, tenantId, ct);

        if (assignment is null)
            return Error.NotFound();

        var result = assignment.Unassign(
            "system" // replace with user when auth is implemented
        );

        if (result.IsError)
            return result.Errors;

        await _repo.UpdateAsync(assignment, ct);

        return assignment;
    }
}
