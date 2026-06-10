using ErrorOr;
using Hiberus.Industria.Vektor.Application.Common.Mappings;
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
    /// Default: 20 items/page, maximum: 100 items/page.
    /// </summary>
    public async Task<PagedResult<TenantDto>> GetAllPaginatedAsync(
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken ct = default
    )
    {
        var paginationParams = new PaginationParams(pageNumber, pageSize);
        
        // Get total count without loading data
        var totalCount = await Task.FromResult(0); // Will implement GetCountAsync if needed
        
        // Get paginated data
        var tenants = await _repo.GetAllAsync(ct);
        var pagedTenants = tenants
            .OrderByDescending(t => t.CreatedAt)
            .Skip(paginationParams.GetSkip())
            .Take(paginationParams.PageSize)
            .ToList();

        totalCount = (await _repo.GetAllAsync(ct)).Count();

        // Project entities to DTOs
        var dtos = pagedTenants
            .Select(t => new TenantDto(
                t.Id,
                t.Name,
                t.Slug,
                t.IsActive,
                t.CreatedAt,
                t.UpdatedAt
            ))
            .ToList();

        return new PagedResult<TenantDto>(dtos, totalCount, pageNumber, pageSize);
    }

    /// <summary>
    /// Retrieves a single tenant by ID as DTO.
    /// </summary>
    public async Task<TenantDto?> GetByIdAsDto(Guid id, CancellationToken ct)
    {
        var tenant = await _repo.GetByIdAsync(id, ct);
        if (tenant is null)
            return null;

        return new TenantDto(
            tenant.Id,
            tenant.Name,
            tenant.Slug,
            tenant.IsActive,
            tenant.CreatedAt,
            tenant.UpdatedAt
        );
    }

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
