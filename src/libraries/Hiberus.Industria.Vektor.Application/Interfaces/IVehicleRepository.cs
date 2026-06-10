using Hiberus.Industria.Vektor.Application.DTOs.Vehicle;
using Hiberus.Industria.Vektor.Domain.Vehicle;

namespace Hiberus.Industria.Vektor.Application.Interfaces;

public interface IVehicleRepository
{
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
