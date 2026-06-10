using ErrorOr;
using Hiberus.Industria.Vektor.Application.Common.Pagination;
using Hiberus.Industria.Vektor.Application.DTOs.Driver;
using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.Driver;

public class DriverAppService
{
    private readonly IDriverRepository _repo;

    public DriverAppService(IDriverRepository repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// Retrieves drivers with pagination, returning DTOs with nested relations.
    /// Uses database-level projections to minimize data transfer and N+1 queries.
    /// </summary>
    public async Task<PagedResult<DriverDto>> GetAllPaginatedAsync(
        Guid tenantId,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken ct = default
    )
    {
        var (dtos, totalCount) = await _repo.GetAllPaginatedAsDtoAsync(
            tenantId,
            pageNumber,
            pageSize,
            ct
        );
        return new PagedResult<DriverDto>(dtos.ToList(), totalCount, pageNumber, pageSize);
    }

    /// <summary>
    /// Retrieves a single driver by ID as DTO with nested relations.
    /// </summary>
    public async Task<DriverDto?> GetByIdAsDto(Guid id, Guid tenantId, CancellationToken ct) =>
        await _repo.GetByIdAsDtoAsync(id, tenantId, ct);

    public async Task<Driver?> GetById(Guid id, Guid tenantId, CancellationToken ct) =>
        await _repo.GetByIdAsync(id, tenantId, ct);

    public async Task<ErrorOr<Driver>> Create(CreateDriverDto dto, Guid tenantId, CancellationToken ct)
    {
        var result = Driver.Create(tenantId, dto.Name, dto.LicenseType, "system", dto.PhoneNumber, dto.LicenseNumber, dto.LicenseExpiryDate);
        if (result.IsError)
            return result.Errors;

        var driver = await _repo.CreateAsync(result.Value, ct);
        return driver;
    }

    public async Task<ErrorOr<Driver>> Update(Guid id, Guid tenantId, UpdateDriverDto dto, CancellationToken ct)
    {
        var driver = await _repo.GetByIdAsync(id, tenantId, ct);
        if (driver is null)
            return Error.NotFound();

        var result = driver.Update(dto.Name, dto.PhoneNumber, dto.LicenseType, dto.LicenseNumber, dto.LicenseExpiryDate, dto.IsAvailable, dto.Timezone, dto.WorkdayStartTime, dto.WorkdayEndTime, "system");
        if (result.IsError)
            return result.Errors;

        await _repo.UpdateAsync(driver, ct);
        return driver;
    }

    public async Task Delete(Guid id, Guid tenantId, CancellationToken ct) =>
        await _repo.DeleteAsync(id, tenantId, ct);
}
