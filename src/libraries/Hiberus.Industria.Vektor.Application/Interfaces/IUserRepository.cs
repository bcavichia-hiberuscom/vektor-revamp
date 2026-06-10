using Hiberus.Industria.Vektor.Application.DTOs.User;
using Hiberus.Industria.Vektor.Domain.User;

namespace Hiberus.Industria.Vektor.Application.Interfaces;

public interface IUserRepository
{
    // ==================== DTO Methods (Optimized Query Projections) ====================

    /// <summary>
    /// Retrieves users as DTOs with pagination, with database-level projections.
    /// All nested relations are projected at the database level for optimal performance.
    /// </summary>
    Task<(IEnumerable<UserDto> Items, int TotalCount)> GetAllPaginatedAsDtoAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    );

    /// <summary>
    /// Retrieves a single user as DTO by ID with database-level projections.
    /// </summary>
    Task<UserDto?> GetByIdAsDtoAsync(Guid id, Guid tenantId, CancellationToken ct = default);

    // ==================== Entity Methods (For CRUD Operations) ====================

    /// <summary>
    /// Retrieves all users for a given tenant with pagination support.
    /// </summary>
    /// <returns>Tuple containing the collection of users and total count.</returns>
    Task<(IEnumerable<User> Items, int TotalCount)> GetAllPaginatedAsync(
        Guid tenantId,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default
    );

    Task<IEnumerable<User>> GetAllAsync(Guid tenantId, CancellationToken ct = default);
    Task<User?> GetByIdAsync(Guid id, Guid tenantId, CancellationToken ct = default);
    Task<User?> GetByEmailAsync(string email, Guid tenantId, CancellationToken ct = default);
    Task<User> CreateAsync(User user, CancellationToken ct = default);
    Task UpdateAsync(User user, CancellationToken ct = default);

    // Soft delete: persiste el estado DeletedAt en lugar de borrar la fila
    Task SoftDeleteAsync(User user, CancellationToken ct = default);
}
