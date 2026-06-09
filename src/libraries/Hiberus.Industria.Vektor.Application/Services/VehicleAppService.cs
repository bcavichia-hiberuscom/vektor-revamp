using ErrorOr;
using Hiberus.Industria.Vektor.Application.DTOs;
using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.Vehicle;

public class VehicleAppService
{
    private readonly IVehicleRepository _repo;

    public VehicleAppService(IVehicleRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Vehicle>> GetAll(Guid tenantId, CancellationToken ct) =>
        await _repo.GetAllAsync(tenantId, ct);

    public async Task<Vehicle?> GetById(Guid id, Guid tenantId, CancellationToken ct) =>
        await _repo.GetByIdAsync(id, tenantId, ct);

    public async Task<ErrorOr<Vehicle>> Create(
        CreateVehicleDto dto,
        Guid tenantId,
        CancellationToken ct
    )
    {
        var result = Vehicle.Create(
            tenantId,
            dto.Label,
            dto.Type,
            "system", //replace with user when auth is implemented
            dto.LicensePlate,
            dto.Brand,
            dto.Model,
            dto.Year
        );

        if (result.IsError)
            return result.Errors;

        var vehicle = await _repo.CreateAsync(result.Value, ct);
        return vehicle;
    }

    public async Task<ErrorOr<Vehicle>> Update(
        Guid id,
        Guid tenantId,
        UpdateVehicleDto dto,
        CancellationToken ct
    )
    {
        var vehicle = await _repo.GetByIdAsync(id, tenantId, ct);
        if (vehicle is null)
            return Error.NotFound();

        var result = vehicle.Update(
            dto.Label,
            dto.LicensePlate,
            dto.Brand,
            dto.Model,
            dto.Year,
            "system"
        );

        if (result.IsError)
            return result.Errors;

        await _repo.UpdateAsync(vehicle, ct);
        return vehicle;
    }

    public async Task Delete(Guid id, Guid tenantId, CancellationToken ct) =>
        await _repo.DeleteAsync(id, tenantId, ct);
}
