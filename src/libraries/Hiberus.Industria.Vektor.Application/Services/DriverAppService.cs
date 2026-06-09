using ErrorOr;
using Hiberus.Industria.Vektor.Application.DTOs;
using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.Driver;

public class DriverAppService
{
    private readonly IDriverRepository _repo;

    public DriverAppService(IDriverRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Driver>> GetAll(Guid tenantId, CancellationToken ct) =>
        await _repo.GetAllAsync(tenantId, ct);

    public async Task<Driver?> GetById(Guid id, Guid tenantId, CancellationToken ct) =>
        await _repo.GetByIdAsync(id, tenantId, ct);

    public async Task<ErrorOr<Driver>> Create(
        CreateDriverDto dto,
        Guid tenantId,
        CancellationToken ct
    )
    {
        var result = Driver.Create(
            tenantId,
            dto.Name,
            dto.LicenseType,
            "system", // replace with user when auth is implemented
            dto.PhoneNumber,
            dto.LicenseNumber,
            dto.LicenseExpiryDate
        );

        if (result.IsError)
            return result.Errors;

        var driver = await _repo.CreateAsync(result.Value, ct);
        return driver;
    }

    public async Task<ErrorOr<Driver>> Update(
        Guid id,
        Guid tenantId,
        UpdateDriverDto dto,
        CancellationToken ct
    )
    {
        var driver = await _repo.GetByIdAsync(id, tenantId, ct);
        if (driver is null)
            return Error.NotFound();

        var result = driver.Update(
            dto.Name,
            dto.PhoneNumber,
            dto.LicenseType,
            dto.LicenseNumber,
            dto.LicenseExpiryDate,
            dto.IsAvailable,
            dto.Timezone,
            dto.WorkdayStartTime,
            dto.WorkdayEndTime,
            "system" // replace with user when auth is implemented
        );

        if (result.IsError)
            return result.Errors;

        await _repo.UpdateAsync(driver, ct);
        return driver;
    }

    public async Task Delete(Guid id, Guid tenantId, CancellationToken ct) =>
        await _repo.DeleteAsync(id, tenantId, ct);
}
