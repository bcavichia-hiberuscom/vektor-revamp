using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.User;
using Hiberus.Industria.Vektor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hiberus.Industria.Vektor.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly VektorDbContext _context;

    public UserRepository(VektorDbContext context)
    {
        _context = context;
    }

    // all the queries should filter out deleted users by default, to avoid mistakes in the application layer.
    private IQueryable<User> ActiveUsers => _context.Users.Where(u => u.DeletedAt == null);

    public async Task<IEnumerable<User>> GetAllAsync(
        Guid tenantId,
        CancellationToken ct = default
    ) => await ActiveUsers.Where(u => u.TenantId == tenantId).ToListAsync(ct);

    public async Task<User?> GetByIdAsync(Guid id, Guid tenantId, CancellationToken ct = default) =>
        await ActiveUsers.FirstOrDefaultAsync(u => u.Id == id && u.TenantId == tenantId, ct);

    public async Task<User?> GetByEmailAsync(
        string email,
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await ActiveUsers.FirstOrDefaultAsync(
            u => u.Email == email.Trim().ToLowerInvariant() && u.TenantId == tenantId,
            ct
        );

    public async Task<User> CreateAsync(User user, CancellationToken ct = default)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync(ct);
        return user;
    }

    public async Task UpdateAsync(User user, CancellationToken ct = default)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(ct);
    }

    public async Task SoftDeleteAsync(User user, CancellationToken ct = default)
    {
        // The domain has already set DeletedAt/DeletedBy/IsActive,
        // we just persist the updated state
        _context.Users.Update(user);
        await _context.SaveChangesAsync(ct);
    }
}
