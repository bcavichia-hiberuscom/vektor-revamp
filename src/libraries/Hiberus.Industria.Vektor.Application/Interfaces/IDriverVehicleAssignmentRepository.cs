using Hiberus.Industria.Vektor.Application.DTOs.DriverVehicleAssignment;
using Hiberus.Industria.Vektor.Domain.DriverVehicleAssignment;

namespace Hiberus.Industria.Vektor.Application.Interfaces;

public interface IDriverVehicleAssignmentRepository
{
    /// <summary>
    /// Retrieves all driver-vehicle assignments for a given tenant with pagination support.
    /// </summary>
    /// <returns>Tuple containing the collection of assignments and total count.</returns>
    Task<(IEnumerable<DriverVehicleAssignment> Items, int TotalCount)> GetAllPaginatedAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    );

    /// <summary>
    /// Retrieves all driver-vehicle assignments for a given tenant as DTOs with pagination support.
    /// Uses database-level projection to minimize data transfer.
    /// </summary>
    /// <returns>Tuple containing the collection of DriverVehicleAssignmentDto and total count.</returns>
    Task<(IEnumerable<DriverVehicleAssignmentDto> Items, int TotalCount)> GetAllPaginatedAsDtoAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    );

    /// <summary>
    /// Retrieves a single driver-vehicle assignment by ID as DTO with eager-loaded relations.
    /// Uses database-level projection for optimal performance.
    /// </summary>
    Task<DriverVehicleAssignmentDto?> GetByIdAsDtoAsync(Guid id, Guid tenantId, CancellationToken ct = default);

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
