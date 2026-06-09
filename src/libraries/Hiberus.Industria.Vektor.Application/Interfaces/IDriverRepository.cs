using Hiberus.Industria.Vektor.Domain.Driver;

namespace Hiberus.Industria.Vektor.Application.Interfaces;

public interface IDriverRepository
{
    Task<IEnumerable<Driver>> GetAllAsync(Guid tenantId, CancellationToken ct = default);
    Task<Driver?> GetByIdAsync(Guid id, Guid tenantId, CancellationToken ct = default);
    Task<Driver> CreateAsync(Driver driver, CancellationToken ct = default);
    Task UpdateAsync(Driver driver, CancellationToken ct = default);
    Task DeleteAsync(Guid id, Guid tenantId, CancellationToken ct = default);
}
