using ErrorOr;
using Hiberus.Industria.Vektor.Application.Common.Mappings;
using Hiberus.Industria.Vektor.Application.Common.Pagination;
using Hiberus.Industria.Vektor.Application.DTOs.User;
using Hiberus.Industria.Vektor.Application.DTOs.Tenant;
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
    /// Default: 20 items/page, maximum: 100 items/page.
    /// </summary>
    public async Task<PagedResult<UserDto>> GetAllPaginatedAsync(
        Guid tenantId,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken ct = default
    )
    {
        var (users, totalCount) = await _repo.GetAllPaginatedAsync(tenantId, pageNumber, pageSize, ct);
        
        // Project entities to DTOs with nested relations
        var dtos = users
            .Select(u => new UserDto(
                u.Id,
                u.TenantId,
                u.Email,
                u.Name,
                u.Role,
                u.IsActive,
                u.CreatedAt,
                u.UpdatedAt,
                new TenantSummaryDto(u.Tenant.Id, u.Tenant.Name, u.Tenant.Slug)
            ))
            .ToList();

        return new PagedResult<UserDto>(dtos, totalCount, pageNumber, pageSize);
    }

    /// <summary>
    /// Retrieves a single user by ID as DTO with nested relations.
    /// </summary>
    public async Task<UserDto?> GetByIdAsDto(Guid id, Guid tenantId, CancellationToken ct)
    {
        var user = await _repo.GetByIdAsync(id, tenantId, ct);
        if (user is null)
            return null;

        return new UserDto(
            user.Id,
            user.TenantId,
            user.Email,
            user.Name,
            user.Role,
            user.IsActive,
            user.CreatedAt,
            user.UpdatedAt,
            new TenantSummaryDto(user.Tenant.Id, user.Tenant.Name, user.Tenant.Slug)
        );
    }

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
