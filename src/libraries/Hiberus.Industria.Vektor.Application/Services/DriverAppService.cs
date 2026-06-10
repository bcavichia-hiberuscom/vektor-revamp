using ErrorOr;
using Hiberus.Industria.Vektor.Application.Common.Mappings;
using Hiberus.Industria.Vektor.Application.Common.Pagination;
using Hiberus.Industria.Vektor.Application.DTOs.Driver;
using Hiberus.Industria.Vektor.Application.DTOs.DriverVehicleAssignment;
using Hiberus.Industria.Vektor.Application.DTOs.Tenant;
using Hiberus.Industria.Vektor.Application.DTOs.Vehicle;
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
    /// Default: 20 items/page, maximum: 100 items/page.
    /// </summary>
    public async Task<PagedResult<DriverDto>> GetAllPaginatedAsync(
        Guid tenantId,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken ct = default
    )
    {
        var (drivers, totalCount) = await _repo.GetAllPaginatedAsync(tenantId, pageNumber, pageSize, ct);
        
        // Project entities to DTOs with nested relations
        var dtos = drivers
            .Select(d => new DriverDto(
                d.Id,
                d.TenantId,
                d.Name,
                d.PhoneNumber,
                d.LicenseType,
                d.LicenseNumber,
                d.LicenseExpiryDate,
                d.IsAvailable,
                d.ImageUrl,
                d.WorkdayStartTime,
                d.WorkdayEndTime,
                d.Timezone,
                new TenantSummaryDto(d.Tenant.Id, d.Tenant.Name, d.Tenant.Slug),
                d.VehicleAssignments
                    .Select(a => new DriverVehicleAssignmentDto(
                        a.Id,
                        a.TenantId,
                        a.DriverId,
                        a.VehicleId,
                        a.AssignedAt,
                        a.UnassignedAt,
                        new DriverSummaryDto(
                            a.Driver.Id,
                            a.Driver.Name,
                            a.Driver.PhoneNumber,
                            a.Driver.LicenseType,
                            a.Driver.IsAvailable
                        ),
                        new VehicleSummaryDto(
                            a.Vehicle.Id,
                            a.Vehicle.Label,
                            a.Vehicle.LicensePlate ?? string.Empty,
                            a.Vehicle.Brand ?? string.Empty,
                            a.Vehicle.Model ?? string.Empty,
                            a.Vehicle.Year ?? 0,
                            a.Vehicle.Type,
                            a.Vehicle.Status.ToString()
                        ),
                        new TenantSummaryDto(a.Tenant.Id, a.Tenant.Name, a.Tenant.Slug)
                    ))
                    .ToList()
            ))
            .ToList();

        return new PagedResult<DriverDto>(dtos, totalCount, pageNumber, pageSize);
    }

    /// <summary>
    /// Retrieves a single driver by ID as DTO with nested relations.
    /// </summary>
    public async Task<DriverDto?> GetByIdAsDto(Guid id, Guid tenantId, CancellationToken ct)
    {
        var driver = await _repo.GetByIdAsync(id, tenantId, ct);
        if (driver is null)
            return null;

        return new DriverDto(
            driver.Id,
            driver.TenantId,
            driver.Name,
            driver.PhoneNumber,
            driver.LicenseType,
            driver.LicenseNumber,
            driver.LicenseExpiryDate,
            driver.IsAvailable,
            driver.ImageUrl,
            driver.WorkdayStartTime,
            driver.WorkdayEndTime,
            driver.Timezone,
            new TenantSummaryDto(driver.Tenant.Id, driver.Tenant.Name, driver.Tenant.Slug),
            driver.VehicleAssignments
                .Select(a => new DriverVehicleAssignmentDto(
                    a.Id,
                    a.TenantId,
                    a.DriverId,
                    a.VehicleId,
                    a.AssignedAt,
                    a.UnassignedAt,
                    new DriverSummaryDto(
                        a.Driver.Id,
                        a.Driver.Name,
                        a.Driver.PhoneNumber,
                        a.Driver.LicenseType,
                        a.Driver.IsAvailable
                    ),
                    new VehicleSummaryDto(
                        a.Vehicle.Id,
                        a.Vehicle.Label,
                        a.Vehicle.LicensePlate ?? string.Empty,
                        a.Vehicle.Brand ?? string.Empty,
                        a.Vehicle.Model ?? string.Empty,
                        a.Vehicle.Year ?? 0,
                        a.Vehicle.Type,
                        a.Vehicle.Status.ToString()
                    ),
                    new TenantSummaryDto(a.Tenant.Id, a.Tenant.Name, a.Tenant.Slug)
                ))
                .ToList()
        );
    }

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
