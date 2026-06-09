using ErrorOr;
using Hiberus.Industria.Vektor.Application.DTOs;
using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.DriverVehicleAssignment;

public class DriverVehicleAssignmentAppService
{
    private readonly IDriverVehicleAssignmentRepository _repo;

    public DriverVehicleAssignmentAppService(IDriverVehicleAssignmentRepository repo)
    {
        _repo = repo;
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
