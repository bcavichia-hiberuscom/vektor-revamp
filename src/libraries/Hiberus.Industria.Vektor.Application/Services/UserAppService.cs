using ErrorOr;
using Hiberus.Industria.Vektor.Application.DTOs;
using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.User;

public class UserAppService
{
    private readonly IUserRepository _repo;

    public UserAppService(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<User>> GetAll(Guid tenantId, CancellationToken ct) =>
        await _repo.GetAllAsync(tenantId, ct);

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
