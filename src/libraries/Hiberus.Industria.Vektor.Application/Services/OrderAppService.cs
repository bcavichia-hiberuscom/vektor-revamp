using ErrorOr;
using Hiberus.Industria.Vektor.Application.DTOs;
using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.Order;

public class OrderAppService
{
    private readonly IOrderRepository _repo;

    public OrderAppService(IOrderRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Order>> GetAll(Guid tenantId, CancellationToken ct) =>
        await _repo.GetAllAsync(tenantId, ct);

    public async Task<Order?> GetById(Guid id, Guid tenantId, CancellationToken ct) =>
        await _repo.GetByIdAsync(id, tenantId, ct);

    public async Task<ErrorOr<Order>> Create(
        CreateOrderDto dto,
        Guid tenantId,
        CancellationToken ct
    )
    {
        var result = Order.Create(
            tenantId,
            dto.Label,
            dto.Latitude,
            dto.Longitude,
            "system", // replace with user when auth is implemented
            dto.ExternalOrderId,
            dto.Description,
            dto.CustomerName,
            dto.CustomerPhone
        );

        if (result.IsError)
            return result.Errors;

        var order = await _repo.CreateAsync(result.Value, ct);
        return order;
    }

    public async Task<ErrorOr<Order>> Update(
        Guid id,
        Guid tenantId,
        UpdateOrderDto dto,
        CancellationToken ct
    )
    {
        var order = await _repo.GetByIdAsync(id, tenantId, ct);
        if (order is null)
            return Error.NotFound();

        var result = order.Update(
            dto.Label,
            dto.Latitude,
            dto.Longitude,
            "system", // replace with user when auth is implemented
            dto.ExternalOrderId,
            dto.Description,
            dto.CustomerName,
            dto.CustomerPhone
        );

        if (result.IsError)
            return result.Errors;

        await _repo.UpdateAsync(order, ct);
        return order;
    }

    public async Task<ErrorOr<Order>> Cancel(Guid id, Guid tenantId, CancellationToken ct)
    {
        var order = await _repo.GetByIdAsync(id, tenantId, ct);
        if (order is null)
            return Error.NotFound();

        var result = order.Cancel("system"); // replace with user when auth is implemented
        if (result.IsError)
            return result.Errors;

        await _repo.UpdateAsync(order, ct);
        return order;
    }

    public async Task Delete(Guid id, Guid tenantId, CancellationToken ct) =>
        await _repo.DeleteAsync(id, tenantId, ct);
}
