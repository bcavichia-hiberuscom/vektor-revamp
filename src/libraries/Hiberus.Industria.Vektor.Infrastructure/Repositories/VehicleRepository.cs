using Hiberus.Industria.Vektor.Application.DTOs;
using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.Entities;
using Hiberus.Industria.Vektor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hiberus.Industria.Vektor.Infrastructure.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly VektorDbContext _context;

    public VehicleRepository(VektorDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Vehicle>> GetAllAsync(
        Guid tenantId,
        CancellationToken ct = default
    ) => await _context.Vehicles.Where(v => v.TenantId == tenantId).ToListAsync(ct);

    public async Task<Vehicle?> GetByIdAsync(
        Guid id,
        Guid tenantId,
        CancellationToken ct = default
    ) => await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id && v.TenantId == tenantId, ct);

    public async Task<Vehicle> CreateAsync(Vehicle vehicle, CancellationToken ct = default)
    {
        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync(ct);
        return vehicle;
    }

    public async Task<Vehicle> UpdateAsync(
        Guid id,
        Guid tenantId,
        UpdateVehicleDto dto,
        CancellationToken ct = default
    )
    {
        var vehicle =
            await _context.Vehicles.FirstOrDefaultAsync(
                v => v.Id == id && v.TenantId == tenantId,
                ct
            ) ?? throw new KeyNotFoundException();

        vehicle.Label = dto.Label;
        vehicle.LicensePlate = dto.LicensePlate;
        vehicle.Brand = dto.Brand;
        vehicle.Model = dto.Model;
        vehicle.Year = dto.Year;
        vehicle.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);
        return vehicle;
    }

    public async Task DeleteAsync(Guid id, Guid tenantId, CancellationToken ct = default)
    {
        var vehicle = await _context.Vehicles.FirstOrDefaultAsync(
            v => v.Id == id && v.TenantId == tenantId,
            ct
        );

        if (vehicle is not null)
        {
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync(ct);
        }
    }
}
