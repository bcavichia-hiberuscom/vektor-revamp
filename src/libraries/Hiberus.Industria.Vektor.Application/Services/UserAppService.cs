using ErrorOr;
using Hiberus.Industria.Vektor.Application.Common.Pagination;
using Hiberus.Industria.Vektor.Application.DTOs.User;
using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.User;

public class UserAppService
{
    private readonly IUserRepository _repo;

    public UserAppService(IUserRepository repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// Retrieves users with pagination, returning DTOs with nested relations.
    /// Uses database-level projections to minimize data transfer and N+1 queries.
    /// Default: 20 items/page, maximum: 100 items/page.
    /// </summary>
    public async Task<PagedResult<UserDto>> GetAllPaginatedAsync(
        Guid tenantId,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken ct = default
    )
    {
        // Repository returns DTOs already projected at database level
        var (dtos, totalCount) = await _repo.GetAllPaginatedAsDtoAsync(
            tenantId,
            pageNumber,
            pageSize,
            ct
        );

        return new PagedResult<UserDto>(dtos.ToList(), totalCount, pageNumber, pageSize);
    }

    /// <summary>
    /// Retrieves a single user by ID as DTO with nested relations.
    /// Uses database-level projection for optimal performance.
    /// </summary>
    public async Task<UserDto?> GetByIdAsDto(Guid id, Guid tenantId, CancellationToken ct) =>
        await _repo.GetByIdAsDtoAsync(id, tenantId, ct);

    public async Task<User?> GetById(Guid id, Guid tenantId, CancellationToken ct) =>
        await _repo.GetByIdAsync(id, tenantId, ct);

    public async Task<ErrorOr<User>> Create(CreateUserDto dto, CancellationToken ct)
    {
        var existing = await _repo.GetByEmailAsync(dto.Email, dto.TenantId, ct);
        if (existing is not null)
            return Error.Conflict(
                "User.Email",
                "A user with this email already exists in this tenant"
            );

        var result = User.Create(
            dto.TenantId,
            dto.Email,
            dto.Name,
            dto.Role,
            "system" // TODO: reemplazar con el usuario autenticado
        );

        if (result.IsError)
            return result.Errors;

        return await _repo.CreateAsync(result.Value, ct);
    }

    public async Task<ErrorOr<User>> Update(
        Guid id,
        Guid tenantId,
        UpdateUserDto dto,
        CancellationToken ct
    )
    {
        var user = await _repo.GetByIdAsync(id, tenantId, ct);
        if (user is null)
            return Error.NotFound("User.NotFound", "User not found");

        var result = user.Update(
            dto.Name,
            dto.Role,
            dto.IsActive,
            "system" // TODO: reemplazar con el usuario autenticado
        );

        if (result.IsError)
            return result.Errors;

        await _repo.UpdateAsync(user, ct);
        return user;
    }

    public async Task<ErrorOr<Deleted>> Delete(Guid id, Guid tenantId, CancellationToken ct)
    {
        var user = await _repo.GetByIdAsync(id, tenantId, ct);
        if (user is null)
            return Error.NotFound("User.NotFound", "User not found");

        var result = user.Delete("system"); // TODO: reemplazar con el usuario autenticado
        if (result.IsError)
            return result.Errors;

        await _repo.SoftDeleteAsync(user, ct);
        return Result.Deleted;
    }
}
