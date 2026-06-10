using System.Linq.Expressions;
using Hiberus.Industria.Vektor.Application.DTOs.Driver;
using Hiberus.Industria.Vektor.Application.DTOs.DriverVehicleAssignment;
using Hiberus.Industria.Vektor.Application.DTOs.Order;
using Hiberus.Industria.Vektor.Application.DTOs.OrderAssignment;
using Hiberus.Industria.Vektor.Application.DTOs.Route;
using Hiberus.Industria.Vektor.Application.DTOs.Tenant;
using Hiberus.Industria.Vektor.Application.DTOs.User;
using Hiberus.Industria.Vektor.Application.DTOs.Vehicle;
using Hiberus.Industria.Vektor.Domain.ActiveVehicleRoute;
using Hiberus.Industria.Vektor.Domain.Driver;
using Hiberus.Industria.Vektor.Domain.DriverVehicleAssignment;
using Hiberus.Industria.Vektor.Domain.Order;
using Hiberus.Industria.Vektor.Domain.OrderAssignment;
using Hiberus.Industria.Vektor.Domain.RouteHistory;
using Hiberus.Industria.Vektor.Domain.Tenant;
using Hiberus.Industria.Vektor.Domain.User;
using Hiberus.Industria.Vektor.Domain.Vehicle;

namespace Hiberus.Industria.Vektor.Application.Common.Mappings;

/// <summary>
/// LINQ projection expressions for mapping domain entities to DTOs with 1-level nesting.
/// Used with .Select() in queries to project data at the database level,
/// avoiding N+1 queries and unnecessary data transfers.
/// </summary>
public static class ProjectionExtensions
{
    // ==================== Summary DTO Projections (no collections) ====================

    /// <summary>
    /// Projects a Tenant entity to TenantSummaryDto (minimal tenant info).
    /// </summary>
    public static Expression<Func<Tenant, TenantSummaryDto>> ToTenantSummaryDtoExpression =>
        tenant => new TenantSummaryDto(tenant.Id, tenant.Name, tenant.Slug);

    /// <summary>
    /// Projects a Vehicle entity to VehicleSummaryDto (minimal vehicle info).
    /// </summary>
    public static Expression<Func<Vehicle, VehicleSummaryDto>> ToVehicleSummaryDtoExpression =>
        vehicle => new VehicleSummaryDto(
            vehicle.Id,
            vehicle.Label,
            vehicle.LicensePlate ?? string.Empty,
            vehicle.Brand ?? string.Empty,
            vehicle.Model ?? string.Empty,
            vehicle.Year ?? 0,
            vehicle.Type,
            vehicle.Status.ToString()
        );

    /// <summary>
    /// Projects a Driver entity to DriverSummaryDto (minimal driver info).
    /// </summary>
    public static Expression<Func<Driver, DriverSummaryDto>> ToDriverSummaryDtoExpression =>
        driver => new DriverSummaryDto(
            driver.Id,
            driver.Name,
            driver.PhoneNumber,
            driver.LicenseType,
            driver.IsAvailable
        );

    /// <summary>
    /// Projects an Order entity to OrderSummaryDto (minimal order info).
    /// </summary>
    public static Expression<Func<Order, OrderSummaryDto>> ToOrderSummaryDtoExpression =>
        order => new OrderSummaryDto(
            order.Id,
            order.Label,
            order.Status.ToString(),
            order.CustomerName
        );

    /// <summary>
    /// Projects a User entity to UserSummaryDto (minimal user info).
    /// </summary>
    public static Expression<Func<User, UserSummaryDto>> ToUserSummaryDtoExpression =>
        user => new UserSummaryDto(user.Id, user.Email, user.Name ?? string.Empty, user.Role);

    // ==================== Full DTO Projections (with nesting) ====================

    /// <summary>
    /// Projects a Tenant entity to TenantDto (full tenant info).
    /// </summary>
    public static Expression<Func<Tenant, TenantDto>> ToTenantDtoExpression =>
        tenant => new TenantDto(
            tenant.Id,
            tenant.Name,
            tenant.Slug,
            tenant.IsActive,
            tenant.CreatedAt,
            tenant.UpdatedAt
        );

    /// <summary>
    /// Projects an Order entity to OrderDto with nested Tenant and Assignments.
    /// Converts OrderStatus enum to string representation.
    /// Note: Must be used after Include(o => o.Tenant, o => o.Assignments) in query.
    /// </summary>
    public static Expression<Func<Order, OrderDto>> ToOrderDtoExpression =>
        order => new OrderDto(
            order.Id,
            order.TenantId,
            order.Label,
            order.Description,
            order.Latitude,
            order.Longitude,
            order.CustomerName,
            order.CustomerPhone,
            order.ExternalOrderId,
            order.Status.ToString(),
            new TenantSummaryDto(order.Tenant.Id, order.Tenant.Name, order.Tenant.Slug),
            order
                .Assignments.Select(a => new OrderAssignmentDto(
                    a.Id,
                    a.TenantId,
                    a.OrderId,
                    a.VehicleId,
                    a.Status.ToString(),
                    a.AssignedAt,
                    a.StartedAt,
                    a.CompletedAt,
                    a.ActualArrival,
                    a.FailureReason,
                    a.CreatedAt,
                    a.UpdatedAt,
                    new OrderSummaryDto(
                        a.Order.Id,
                        a.Order.Label,
                        a.Order.Status.ToString(),
                        a.Order.CustomerName
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

    /// <summary>
    /// Projects a Vehicle entity to VehicleDto with nested Tenant and Assignments.
    /// Note: Must be used after Include(v => v.Tenant, v => v.Assignments, v => v.OrderAssignments) in query.
    /// </summary>
    public static Expression<Func<Vehicle, VehicleDto>> ToVehicleDtoExpression =>
        vehicle => new VehicleDto(
            vehicle.Id,
            vehicle.TenantId,
            vehicle.Label,
            vehicle.LicensePlate,
            vehicle.Brand,
            vehicle.Model,
            vehicle.Year,
            vehicle.Type,
            vehicle.Status.ToString(),
            new TenantSummaryDto(vehicle.Tenant.Id, vehicle.Tenant.Name, vehicle.Tenant.Slug),
            vehicle
                .Assignments.Select(a => new DriverVehicleAssignmentDto(
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
                .ToList(),
            vehicle
                .OrderAssignments.Select(a => new OrderAssignmentDto(
                    a.Id,
                    a.TenantId,
                    a.OrderId,
                    a.VehicleId,
                    a.Status.ToString(),
                    a.AssignedAt,
                    a.StartedAt,
                    a.CompletedAt,
                    a.ActualArrival,
                    a.FailureReason,
                    a.CreatedAt,
                    a.UpdatedAt,
                    new OrderSummaryDto(
                        a.Order.Id,
                        a.Order.Label,
                        a.Order.Status.ToString(),
                        a.Order.CustomerName
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

    /// <summary>
    /// Projects a Driver entity to DriverDto with nested Tenant and VehicleAssignments.
    /// Note: Must be used after Include(d => d.Tenant, d => d.VehicleAssignments) in query.
    /// </summary>
    public static Expression<Func<Driver, DriverDto>> ToDriverDtoExpression =>
        driver => new DriverDto(
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
            driver
                .VehicleAssignments.Select(a => new DriverVehicleAssignmentDto(
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

    /// <summary>
    /// Projects a User entity to UserDto with nested Tenant.
    /// Note: Must be used after Include(u => u.Tenant) in query.
    /// </summary>
    public static Expression<Func<User, UserDto>> ToUserDtoExpression =>
        user => new UserDto(
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

    /// <summary>
    /// Projects an OrderAssignment entity to OrderAssignmentDto with nested Order, Vehicle, and Tenant.
    /// Note: Must be used after Include(a => a.Order, a => a.Vehicle, a => a.Tenant) in query.
    /// </summary>
    public static Expression<
        Func<OrderAssignment, OrderAssignmentDto>
    > ToOrderAssignmentDtoExpression =>
        assignment => new OrderAssignmentDto(
            assignment.Id,
            assignment.TenantId,
            assignment.OrderId,
            assignment.VehicleId,
            assignment.Status.ToString(),
            assignment.AssignedAt,
            assignment.StartedAt,
            assignment.CompletedAt,
            assignment.ActualArrival,
            assignment.FailureReason,
            assignment.CreatedAt,
            assignment.UpdatedAt,
            new OrderSummaryDto(
                assignment.Order.Id,
                assignment.Order.Label,
                assignment.Order.Status.ToString(),
                assignment.Order.CustomerName
            ),
            new VehicleSummaryDto(
                assignment.Vehicle.Id,
                assignment.Vehicle.Label,
                assignment.Vehicle.LicensePlate ?? string.Empty,
                assignment.Vehicle.Brand ?? string.Empty,
                assignment.Vehicle.Model ?? string.Empty,
                assignment.Vehicle.Year ?? 0,
                assignment.Vehicle.Type,
                assignment.Vehicle.Status.ToString()
            ),
            new TenantSummaryDto(
                assignment.Tenant.Id,
                assignment.Tenant.Name,
                assignment.Tenant.Slug
            )
        );

    /// <summary>
    /// Projects a DriverVehicleAssignment entity to DriverVehicleAssignmentDto with nested Driver, Vehicle, and Tenant.
    /// Note: Must be used after Include(a => a.Driver, a => a.Vehicle, a => a.Tenant) in query.
    /// </summary>
    public static Expression<
        Func<DriverVehicleAssignment, DriverVehicleAssignmentDto>
    > ToDriverVehicleAssignmentDtoExpression =>
        assignment => new DriverVehicleAssignmentDto(
            assignment.Id,
            assignment.TenantId,
            assignment.DriverId,
            assignment.VehicleId,
            assignment.AssignedAt,
            assignment.UnassignedAt,
            new DriverSummaryDto(
                assignment.Driver.Id,
                assignment.Driver.Name,
                assignment.Driver.PhoneNumber,
                assignment.Driver.LicenseType,
                assignment.Driver.IsAvailable
            ),
            new VehicleSummaryDto(
                assignment.Vehicle.Id,
                assignment.Vehicle.Label,
                assignment.Vehicle.LicensePlate ?? string.Empty,
                assignment.Vehicle.Brand ?? string.Empty,
                assignment.Vehicle.Model ?? string.Empty,
                assignment.Vehicle.Year ?? 0,
                assignment.Vehicle.Type,
                assignment.Vehicle.Status.ToString()
            ),
            new TenantSummaryDto(
                assignment.Tenant.Id,
                assignment.Tenant.Name,
                assignment.Tenant.Slug
            )
        );

    /// <summary>
    /// Projects an ActiveVehicleRoute entity to ActiveVehicleRouteDto with nested Vehicle and Tenant.
    /// Note: Must be used after Include(r => r.Vehicle, r => r.Tenant) in query.
    /// </summary>
    public static Expression<
        Func<ActiveVehicleRoute, ActiveVehicleRouteDto>
    > ToActiveVehicleRouteDtoExpression =>
        route => new ActiveVehicleRouteDto(
            route.Id,
            route.TenantId,
            route.VehicleId,
            route.RoutePayload,
            route.AssociatedOrderIds,
            route.StartedAt,
            route.CreatedAt,
            new VehicleSummaryDto(
                route.Vehicle.Id,
                route.Vehicle.Label,
                route.Vehicle.LicensePlate ?? string.Empty,
                route.Vehicle.Brand ?? string.Empty,
                route.Vehicle.Model ?? string.Empty,
                route.Vehicle.Year ?? 0,
                route.Vehicle.Type,
                route.Vehicle.Status.ToString()
            ),
            new TenantSummaryDto(route.Tenant.Id, route.Tenant.Name, route.Tenant.Slug)
        );

    /// <summary>
    /// Projects a RouteHistory entity to RouteHistoryDto with nested Vehicle and Tenant.
    /// Note: Must be used after Include(r => r.Vehicle, r => r.Tenant) in query.
    /// </summary>
    public static Expression<Func<RouteHistory, RouteHistoryDto>> ToRouteHistoryDtoExpression =>
        history => new RouteHistoryDto(
            history.Id,
            history.TenantId,
            history.VehicleId,
            history.RoutePayload,
            history.AssociatedOrderIds,
            history.StartedAt,
            history.FinishedAt,
            history.FinishedAt - history.StartedAt,
            new VehicleSummaryDto(
                history.Vehicle.Id,
                history.Vehicle.Label,
                history.Vehicle.LicensePlate ?? string.Empty,
                history.Vehicle.Brand ?? string.Empty,
                history.Vehicle.Model ?? string.Empty,
                history.Vehicle.Year ?? 0,
                history.Vehicle.Type,
                history.Vehicle.Status.ToString()
            ),
            new TenantSummaryDto(history.Tenant.Id, history.Tenant.Name, history.Tenant.Slug)
        );

    // ==================== Extension Methods for Queryable ====================

    /// <summary>
    /// Projects an IQueryable of Tenant to IQueryable of TenantDto.
    /// Use this method when building queries to ensure projection at DB level.
    /// </summary>
    public static IQueryable<TenantDto> SelectToTenantDto(this IQueryable<Tenant> query) =>
        query.Select(ToTenantDtoExpression);

    /// <summary>
    /// Projects an IQueryable of Order to IQueryable of OrderDto with nested relations.
    /// Use this method when building queries to ensure projection at DB level.
    /// </summary>
    public static IQueryable<OrderDto> SelectToOrderDto(this IQueryable<Order> query) =>
        query.Select(ToOrderDtoExpression);

    /// <summary>
    /// Projects an IQueryable of Vehicle to IQueryable of VehicleDto with nested relations.
    /// Use this method when building queries to ensure projection at DB level.
    /// </summary>
    public static IQueryable<VehicleDto> SelectToVehicleDto(this IQueryable<Vehicle> query) =>
        query.Select(ToVehicleDtoExpression);

    /// <summary>
    /// Projects an IQueryable of Driver to IQueryable of DriverDto with nested relations.
    /// Use this method when building queries to ensure projection at DB level.
    /// </summary>
    public static IQueryable<DriverDto> SelectToDriverDto(this IQueryable<Driver> query) =>
        query.Select(ToDriverDtoExpression);

    /// <summary>
    /// Projects an IQueryable of User to IQueryable of UserDto with nested relations.
    /// Use this method when building queries to ensure projection at DB level.
    /// </summary>
    public static IQueryable<UserDto> SelectToUserDto(this IQueryable<User> query) =>
        query.Select(ToUserDtoExpression);

    /// <summary>
    /// Projects an IQueryable of OrderAssignment to IQueryable of OrderAssignmentDto with nested relations.
    /// Use this method when building queries to ensure projection at DB level.
    /// </summary>
    public static IQueryable<OrderAssignmentDto> SelectToOrderAssignmentDto(
        this IQueryable<OrderAssignment> query
    ) => query.Select(ToOrderAssignmentDtoExpression);

    /// <summary>
    /// Projects an IQueryable of DriverVehicleAssignment to IQueryable of DriverVehicleAssignmentDto with nested relations.
    /// Use this method when building queries to ensure projection at DB level.
    /// </summary>
    public static IQueryable<DriverVehicleAssignmentDto> SelectToDriverVehicleAssignmentDto(
        this IQueryable<DriverVehicleAssignment> query
    ) => query.Select(ToDriverVehicleAssignmentDtoExpression);

    /// <summary>
    /// Projects an IQueryable of ActiveVehicleRoute to IQueryable of ActiveVehicleRouteDto with nested relations.
    /// Use this method when building queries to ensure projection at DB level.
    /// </summary>
    public static IQueryable<ActiveVehicleRouteDto> SelectToActiveVehicleRouteDto(
        this IQueryable<ActiveVehicleRoute> query
    ) => query.Select(ToActiveVehicleRouteDtoExpression);

    /// <summary>
    /// Projects an IQueryable of RouteHistory to IQueryable of RouteHistoryDto with nested relations.
    /// Use this method when building queries to ensure projection at DB level.
    /// </summary>
    public static IQueryable<RouteHistoryDto> SelectToRouteHistoryDto(
        this IQueryable<RouteHistory> query
    ) => query.Select(ToRouteHistoryDtoExpression);
}
