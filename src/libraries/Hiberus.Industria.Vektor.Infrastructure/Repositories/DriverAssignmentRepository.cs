using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.DriverVehicleAssignment;
using Hiberus.Industria.Vektor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hiberus.Industria.Vektor.Infrastructure.Repositories;

public class DriverVehicleAssignmentRepository : IDriverVehicleAssignmentRepository
{
    private readonly VektorDbContext _context;

    public DriverVehicleAssignmentRepository(VektorDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DriverVehicleAssignment>> GetByTenantAsync(
        Guid tenantId,
        CancellationToken ct = default
    ) => await _context.DriverVehicleAssignments.Where(a => a.TenantId == tenantId).ToListAsync(ct);

    public async Task<DriverVehicleAssignment?> GetByIdAsync(
        Guid id,
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context.DriverVehicleAssignments.FirstOrDefaultAsync(
            a => a.Id == id && a.TenantId == tenantId,
            ct
        );

    public async Task<IEnumerable<DriverVehicleAssignment>> GetByDriverAsync(
        Guid driverId,
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .DriverVehicleAssignments.Where(a => a.DriverId == driverId && a.TenantId == tenantId)
            .ToListAsync(ct);

    public async Task<IEnumerable<DriverVehicleAssignment>> GetByVehicleAsync(
        Guid vehicleId,
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await _context
            .DriverVehicleAssignments.Where(a => a.VehicleId == vehicleId && a.TenantId == tenantId)
            .ToListAsync(ct);

    public async Task<DriverVehicleAssignment> CreateAsync(
        DriverVehicleAssignment assignment,
        CancellationToken ct = default
    )
    {
        _context.DriverVehicleAssignments.Add(assignment);
        await _context.SaveChangesAsync(ct);
        return assignment;
    }

    public async Task UpdateAsync(
        DriverVehicleAssignment assignment,
        CancellationToken ct = default
    )
    {
        _context.DriverVehicleAssignments.Update(assignment);
        await _context.SaveChangesAsync(ct);
    }
}
