using ErrorOr;
using Hiberus.Industria.Vektor.Application.Common.Pagination;
using Hiberus.Industria.Vektor.Application.DTOs.OrderAssignment;
using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Domain.OrderAssignment;

public class OrderAssignmentAppService
{
    private readonly IOrderAssignmentRepository _repo;

    public OrderAssignmentAppService(IOrderAssignmentRepository repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// Retrieves paginated order assignments for a tenant as DTOs.
    /// Uses database-level projection for optimal performance.
    /// </summary>
    public async Task<PagedResult<OrderAssignmentDto>> GetAllPaginatedAsync(
        Guid tenantId,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken ct = default
    )
    {
        var (dtos, totalCount) = await _repo.GetAllPaginatedAsDtoAsync(tenantId, pageNumber, pageSize, ct);
        return new PagedResult<OrderAssignmentDto>(dtos.ToList(), totalCount, pageNumber, pageSize);
    }

    /// <summary>
    /// Retrieves a single order assignment by ID as DTO.
    /// </summary>
    public async Task<OrderAssignmentDto?> GetByIdAsDto(Guid id, Guid tenantId, CancellationToken ct) =>
        await _repo.GetByIdAsDtoAsync(id, tenantId, ct);

    public async Task<IEnumerable<OrderAssignment>> GetByTenant(
        Guid tenantId,
        CancellationToken ct
    ) => await _repo.GetByTenantAsync(tenantId, ct);

    public async Task<IEnumerable<OrderAssignment>> GetByOrder(
        Guid orderId,
        Guid tenantId,
        CancellationToken ct
    ) => await _repo.GetByOrderAsync(orderId, tenantId, ct);

    public async Task<IEnumerable<OrderAssignmentDto>> GetByOrderAsDto(
        Guid orderId,
        Guid tenantId,
        CancellationToken ct
    ) => await _repo.GetByOrderAsDtoAsync(orderId, tenantId, ct);

    public async Task<IEnumerable<OrderAssignment>> GetByVehicle(
        Guid vehicleId,
        Guid tenantId,
        CancellationToken ct
    ) => await _repo.GetByVehicleAsync(vehicleId, tenantId, ct);

    public async Task<IEnumerable<OrderAssignmentDto>> GetByVehicleAsDto(
        Guid vehicleId,
        Guid tenantId,
        CancellationToken ct
    ) => await _repo.GetByVehicleAsDtoAsync(vehicleId, tenantId, ct);

    public async Task<ErrorOr<OrderAssignment>> Create(
        CreateOrderAssignmentDto dto,
        Guid tenantId,
        CancellationToken ct
    )
    {
        var result = OrderAssignment.Create(
            tenantId,
            dto.OrderId,
            dto.VehicleId,
            "system" // TODO: replace with authenticated user
        );

        if (result.IsError)
            return result.Errors;

        return await _repo.CreateAsync(result.Value, ct);
    }

    public async Task<ErrorOr<OrderAssignment>> Start(Guid id, Guid tenantId, CancellationToken ct)
    {
        var assignment = await _repo.GetByIdAsync(id, tenantId, ct);
        if (assignment is null)
            return Error.NotFound("OrderAssignment.NotFound", "Assignment not found");

        var result = assignment.Start("system"); // TODO: replace with authenticated user
        if (result.IsError)
            return result.Errors;

        await _repo.UpdateAsync(assignment, ct);
        return assignment;
    }

    public async Task<ErrorOr<OrderAssignment>> Complete(
        Guid id,
        Guid tenantId,
        CompleteOrderAssignmentDto dto,
        CancellationToken ct
    )
    {
        var assignment = await _repo.GetByIdAsync(id, tenantId, ct);
        if (assignment is null)
            return Error.NotFound("OrderAssignment.NotFound", "Assignment not found");

        var result = assignment.Complete(dto.ActualArrival, "system"); // TODO: replace with authenticated user
        if (result.IsError)
            return result.Errors;

        await _repo.UpdateAsync(assignment, ct);
        return assignment;
    }

    public async Task<ErrorOr<OrderAssignment>> Fail(
        Guid id,
        Guid tenantId,
        FailOrderAssignmentDto dto,
        CancellationToken ct
    )
    {
        var assignment = await _repo.GetByIdAsync(id, tenantId, ct);
        if (assignment is null)
            return Error.NotFound("OrderAssignment.NotFound", "Assignment not found");

        var result = assignment.Fail(dto.Reason, "system"); // TODO: replace with authenticated user
        if (result.IsError)
            return result.Errors;

        await _repo.UpdateAsync(assignment, ct);
        return assignment;
    }
}
