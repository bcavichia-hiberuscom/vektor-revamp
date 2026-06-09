using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.OrderAssignment;
using Hiberus.Industria.Vektor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hiberus.Industria.Vektor.Infrastructure.Repositories;

public class OrderAssignmentRepository : IOrderAssignmentRepository
{
    private readonly VektorDbContext _context;

    public OrderAssignmentRepository(VektorDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<OrderAssignment>> GetByTenantAsync(
        Guid tenantId,
        CancellationToken ct = default
    )
    {
        return await _context
            .OrderAssignments.Where(a => a.TenantId == tenantId)
            .OrderByDescending(a => a.AssignedAt)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<OrderAssignment>> GetByOrderAsync(
        Guid orderId,
        Guid tenantId,
        CancellationToken ct = default
    )
    {
        return await _context
            .OrderAssignments.Where(a => a.OrderId == orderId && a.TenantId == tenantId)
            .OrderByDescending(a => a.AssignedAt)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<OrderAssignment>> GetByVehicleAsync(
        Guid vehicleId,
        Guid tenantId,
        CancellationToken ct = default
    )
    {
        return await _context
            .OrderAssignments.Where(a => a.VehicleId == vehicleId && a.TenantId == tenantId)
            .OrderByDescending(a => a.AssignedAt)
            .ToListAsync(ct);
    }

    public async Task<OrderAssignment?> GetByIdAsync(
        Guid id,
        Guid tenantId,
        CancellationToken ct = default
    )
    {
        return await _context.OrderAssignments.FirstOrDefaultAsync(
            a => a.Id == id && a.TenantId == tenantId,
            ct
        );
    }

    public async Task<IEnumerable<OrderAssignment>> GetByVehicleAsync(
        Guid vehicleId,
        CancellationToken ct = default
    ) =>
        await _context
            .OrderAssignments.Where(a => a.VehicleId == vehicleId)
            .OrderByDescending(a => a.AssignedAt)
            .ToListAsync(ct);

    public async Task<OrderAssignment?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _context.OrderAssignments.FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task<OrderAssignment> CreateAsync(
        OrderAssignment assignment,
        CancellationToken ct = default
    )
    {
        _context.OrderAssignments.Add(assignment);
        await _context.SaveChangesAsync(ct);
        return assignment;
    }

    public async Task UpdateAsync(OrderAssignment assignment, CancellationToken ct = default)
    {
        _context.OrderAssignments.Update(assignment);
        await _context.SaveChangesAsync(ct);
    }
}
