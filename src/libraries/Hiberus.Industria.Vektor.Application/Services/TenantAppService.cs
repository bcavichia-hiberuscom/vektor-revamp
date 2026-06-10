using ErrorOr;
using Hiberus.Industria.Vektor.Application.Common.Pagination;
using Hiberus.Industria.Vektor.Application.DTOs.Tenant;
using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.Tenant;

public class TenantAppService
{
    private readonly ITenantRepository _repo;

    public TenantAppService(ITenantRepository repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// Retrieves all tenants with pagination, returning DTOs.
    /// Uses database-level projections to minimize data transfer.
    /// Default: 20 items/page, maximum: 100 items/page.
    /// </summary>
    public async Task<PagedResult<TenantDto>> GetAllPaginatedAsync(
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken ct = default
    )
    {
        // Repository returns DTOs already projected at database level
        var (dtos, totalCount) = await _repo.GetAllPaginatedAsDtoAsync(
            pageNumber,
            pageSize,
            ct
        );

        return new PagedResult<TenantDto>(dtos.ToList(), totalCount, pageNumber, pageSize);
    }

    /// <summary>
    /// Retrieves a single tenant by ID as DTO.
    /// Uses database-level projection for optimal performance.
    /// </summary>
    public async Task<TenantDto?> GetByIdAsDto(Guid id, CancellationToken ct) =>
        await _repo.GetByIdAsDtoAsync(id, ct);

    public async Task<Tenant?> GetById(Guid id, CancellationToken ct) =>
        await _repo.GetByIdAsync(id, ct);

    public async Task<ErrorOr<Tenant>> Create(CreateTenantDto dto, CancellationToken ct)
    {
        // Globally unique slug (not scoped to tenantId like the rest of entities)
        var existing = await _repo.GetBySlugAsync(dto.Slug, ct);
        if (existing is not null)
            return Error.Conflict("Tenant.Slug", "A tenant with this slug already exists");

        var result = Tenant.Create(
            dto.Name,
            dto.Slug,
            "system" // TODO: replace with authenticated user
        );

        if (result.IsError)
            return result.Errors;

        return await _repo.CreateAsync(result.Value, ct);
    }

    public async Task<ErrorOr<Tenant>> Update(Guid id, UpdateTenantDto dto, CancellationToken ct)
    {
        var tenant = await _repo.GetByIdAsync(id, ct);
        if (tenant is null)
            return Error.NotFound("Tenant.NotFound", "Tenant not found");

        var result = tenant.Update(
            dto.Name,
            "system" // TODO: replace with authenticated user
        );

        if (result.IsError)
            return result.Errors;

        await _repo.UpdateAsync(tenant, ct);
        return tenant;
    }

    public async Task<ErrorOr<Deleted>> Delete(Guid id, CancellationToken ct)
    {
        var tenant = await _repo.GetByIdAsync(id, ct);
        if (tenant is null)
            return Error.NotFound("Tenant.NotFound", "Tenant not found");

        var result = tenant.Delete("system"); // TODO: replace with authenticated user
        if (result.IsError)
            return result.Errors;

        await _repo.SoftDeleteAsync(tenant, ct);
        return Result.Deleted;
    }
}
