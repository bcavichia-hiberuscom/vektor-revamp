using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.Order;
using Hiberus.Industria.Vektor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hiberus.Industria.Vektor.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly VektorDbContext _context;

    public OrderRepository(VektorDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetAllAsync(
        Guid tenantId,
        CancellationToken ct = default
    ) => await _context.Orders.Where(o => o.TenantId == tenantId).ToListAsync(ct);

    public async Task<Order?> GetByIdAsync(
        Guid id,
        Guid tenantId,
        CancellationToken ct = default
    ) => await _context.Orders.FirstOrDefaultAsync(o => o.Id == id && o.TenantId == tenantId, ct);

    public async Task<Order> CreateAsync(Order order, CancellationToken ct = default)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync(ct);
        return order;
    }

    public async Task UpdateAsync(Order order, CancellationToken ct = default)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, Guid tenantId, CancellationToken ct = default)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(
            o => o.Id == id && o.TenantId == tenantId,
            ct
        );

        if (order is not null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync(ct);
        }
    }
}
