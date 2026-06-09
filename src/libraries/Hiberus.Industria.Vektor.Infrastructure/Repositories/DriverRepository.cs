using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.Driver;
using Hiberus.Industria.Vektor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hiberus.Industria.Vektor.Infrastructure.Repositories;

public class DriverRepository : IDriverRepository
{
    private readonly VektorDbContext _context;

    public DriverRepository(VektorDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Driver>> GetAllAsync(
        Guid tenantId,
        CancellationToken ct = default
    ) => await _context.Drivers.Where(d => d.TenantId == tenantId).ToListAsync(ct);

    public async Task<Driver?> GetByIdAsync(
        Guid id,
        Guid tenantId,
        CancellationToken ct = default
    ) => await _context.Drivers.FirstOrDefaultAsync(d => d.Id == id && d.TenantId == tenantId, ct);

    public async Task<Driver> CreateAsync(Driver driver, CancellationToken ct = default)
    {
        _context.Drivers.Add(driver);
        await _context.SaveChangesAsync(ct);
        return driver;
    }

    public async Task UpdateAsync(Driver driver, CancellationToken ct = default)
    {
        _context.Drivers.Update(driver);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, Guid tenantId, CancellationToken ct = default)
    {
        var driver = await _context.Drivers.FirstOrDefaultAsync(
            d => d.Id == id && d.TenantId == tenantId,
            ct
        );

        if (driver is not null)
        {
            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync(ct);
        }
    }
}
