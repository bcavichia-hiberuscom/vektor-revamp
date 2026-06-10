using Hiberus.Industria.Vektor.Application.Common.Mappings;
using Hiberus.Industria.Vektor.Application.Common.Pagination;
using Hiberus.Industria.Vektor.Application.DTOs.User;
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

    /// <summary>
    /// Query filter: Only active users (not soft-deleted).
    /// </summary>
    // All queries should filter out deleted users by default, to avoid mistakes in the application layer.
    private IQueryable<User> ActiveUsers =>
        _context.Users.AsNoTracking().Where(u => u.DeletedAt == null);

    // ==================== DTO Methods (Database-Level Projections) ====================

    /// <summary>
    /// Retrieves users as DTOs with pagination using database-level projections.
    /// All nested relations (Tenant) are projected at the database level for optimal performance.
    /// Filters only active users for the specified tenant.
    /// </summary>
    public async Task<(IEnumerable<UserDto> Items, int TotalCount)> GetAllPaginatedAsDtoAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var paginationParams = new PaginationParams(pageNumber, pageSize);

        // Count total records matching the filter
        var totalCount = await ActiveUsers.Where(u => u.TenantId == tenantId).CountAsync(ct);

        // Retrieve paginated data with required relationships for projection
        var items = await ActiveUsers
            .Where(u => u.TenantId == tenantId)
            // Include all required relations for the UserDto projection
            .Include(u => u.Tenant)
            .OrderByDescending(u => u.CreatedAt)
            .Skip(paginationParams.GetSkip())
            .Take(paginationParams.PageSize)
            // Project to DTO at database level using compiled LINQ expressions
            .Select(ProjectionExtensions.ToUserDtoExpression)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    /// <summary>
    /// Retrieves a single user as DTO by ID using database-level projection.
    /// All nested relations are projected at database level for performance.
    /// </summary>
    public async Task<UserDto?> GetByIdAsDtoAsync(
        Guid id,
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await ActiveUsers
            .Where(u => u.Id == id && u.TenantId == tenantId)
            // Include all required relations for the UserDto projection
            .Include(u => u.Tenant)
            // Project to DTO at database level using compiled LINQ expressions
            .Select(ProjectionExtensions.ToUserDtoExpression)
            .FirstOrDefaultAsync(ct);

    /// <summary>
    /// Retrieves users with pagination, filtering only active users for a tenant.
    /// Eager-loads related Tenant.
    /// Uses AsNoTracking() for read-only queries to improve performance.
    /// </summary>
    public async Task<(IEnumerable<User> Items, int TotalCount)> GetAllPaginatedAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var paginationParams = new PaginationParams(pageNumber, pageSize);

        // Get total count without loading data
        var totalCount = await ActiveUsers.Where(u => u.TenantId == tenantId).CountAsync(ct);

        // Get paginated data with eager-loaded relations
        var items = await ActiveUsers
            .Where(u => u.TenantId == tenantId)
            .Include(u => u.Tenant)
            .OrderByDescending(u => u.CreatedAt)
            .Skip(paginationParams.GetSkip())
            .Take(paginationParams.PageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    /// <summary>
    /// Retrieves all active users for a tenant with eager-loaded relations.
    /// Use with caution - consider GetAllPaginatedAsync for large datasets.
    /// </summary>
    public async Task<IEnumerable<User>> GetAllAsync(
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await ActiveUsers
            .Where(u => u.TenantId == tenantId)
            .Include(u => u.Tenant)
            .OrderByDescending(u => u.CreatedAt)
            .ToListAsync(ct);

    /// <summary>
    /// Retrieves a single user by ID with eager-loaded relations, filtering by active users only.
    /// </summary>
    public async Task<User?> GetByIdAsync(Guid id, Guid tenantId, CancellationToken ct = default) =>
        await ActiveUsers
            .Where(u => u.Id == id && u.TenantId == tenantId)
            .Include(u => u.Tenant)
            .FirstOrDefaultAsync(ct);

    /// <summary>
    /// Retrieves a single user by email with eager-loaded relations, filtering by active users only.
    /// </summary>
    public async Task<User?> GetByEmailAsync(
        string email,
        Guid tenantId,
        CancellationToken ct = default
    ) =>
        await ActiveUsers
            .Where(u => u.Email == email.Trim().ToLowerInvariant() && u.TenantId == tenantId)
            .Include(u => u.Tenant)
            .FirstOrDefaultAsync(ct);

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

    /// <summary>
    /// Soft delete: Marks user as deleted without removing from database.
    /// </summary>
    public async Task SoftDeleteAsync(User user, CancellationToken ct = default)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, Guid tenantId, CancellationToken ct = default)
    {
        var user = await _context.Users.FirstOrDefaultAsync(
            u => u.Id == id && u.TenantId == tenantId,
            ct
        );

        if (user is not null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(ct);
        }
    }
}
