using Vektor.API.Application.DTOs;
using Vektor.API.Domain.Entities;

namespace Vektor.API.Application.Interfaces;

public interface IVehicleRepository
{
    Task<IEnumerable<Vehicle>> GetAllAsync(Guid tenantId, CancellationToken ct = default);
    Task<Vehicle?> GetByIdAsync(Guid id, Guid tenantId, CancellationToken ct = default);
    Task<Vehicle> CreateAsync(Vehicle vehicle, CancellationToken ct = default);
    Task<Vehicle> UpdateAsync(
        Guid id,
        Guid tenantId,
        UpdateVehicleDto dto,
        CancellationToken ct = default
    );
    Task DeleteAsync(Guid id, Guid tenantId, CancellationToken ct = default);
}

//todo falta servicio