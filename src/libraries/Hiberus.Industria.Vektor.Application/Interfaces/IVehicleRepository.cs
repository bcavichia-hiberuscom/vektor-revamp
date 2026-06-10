using Hiberus.Industria.Vektor.Application.DTOs.Vehicle;
using Hiberus.Industria.Vektor.Domain.Vehicle;

namespace Hiberus.Industria.Vektor.Application.Interfaces;

public interface IVehicleRepository
{
    // ==================== DTO Methods (Optimized Query Projections) ====================

    /// <summary>
    /// Retrieves vehicles as DTOs with pagination, with database-level projections.
    /// All nested relations are projected at the database level for optimal performance.
    /// </summary>
    Task<(IEnumerable<VehicleDto> Items, int TotalCount)> GetAllPaginatedAsDtoAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    );

    /// <summary>
    /// Retrieves a single vehicle as DTO by ID with database-level projections.
    /// </summary>
    Task<VehicleDto?> GetByIdAsDtoAsync(Guid id, Guid tenantId, CancellationToken ct = default);

    // ==================== Entity Methods (For CRUD Operations) ====================

    /// <summary>
    /// Retrieves all vehicles for a given tenant with pagination support.
    /// </summary>
    /// <returns>Tuple containing the collection of vehicles and total count of vehicles.</returns>
    Task<(IEnumerable<Vehicle> Items, int TotalCount)> GetAllPaginatedAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    );

    Task<IEnumerable<Vehicle>> GetAllAsync(Guid tenantId, CancellationToken ct = default);
    Task<Vehicle?> GetByIdAsync(Guid id, Guid tenantId, CancellationToken ct = default);
    Task<Vehicle> CreateAsync(Vehicle vehicle, CancellationToken ct = default);
    Task UpdateAsync(Vehicle vehicle, CancellationToken ct = default);
    Task DeleteAsync(Guid id, Guid tenantId, CancellationToken ct = default);
}
