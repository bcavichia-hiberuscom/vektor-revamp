using Hiberus.Industria.Vektor.Domain.DriverVehicleAssignment;

namespace Hiberus.Industria.Vektor.Application.Interfaces;

public interface IDriverVehicleAssignmentRepository
{
    Task<IEnumerable<DriverVehicleAssignment>> GetByTenantAsync(
        Guid tenantId,
        CancellationToken ct = default
    );

    Task<DriverVehicleAssignment?> GetByIdAsync(
        Guid id,
        Guid tenantId,
        CancellationToken ct = default
    );

    Task<IEnumerable<DriverVehicleAssignment>> GetByDriverAsync(
        Guid driverId,
        Guid tenantId,
        CancellationToken ct = default
    );

    Task<IEnumerable<DriverVehicleAssignment>> GetByVehicleAsync(
        Guid vehicleId,
        Guid tenantId,
        CancellationToken ct = default
    );

    Task<DriverVehicleAssignment> CreateAsync(
        DriverVehicleAssignment assignment,
        CancellationToken ct = default
    );

    Task UpdateAsync(DriverVehicleAssignment assignment, CancellationToken ct = default);
}
