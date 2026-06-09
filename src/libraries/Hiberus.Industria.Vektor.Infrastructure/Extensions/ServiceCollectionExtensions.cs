using Hiberus.Industria.Vektor.Application.Interfaces;
using Hiberus.Industria.Vektor.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Hiberus.Industria.Vektor.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<VehicleAppService>();
        services.AddScoped<IDriverRepository, DriverRepository>();
        services.AddScoped<DriverAppService>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<OrderAppService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<UserAppService>();
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<TenantAppService>();
        services.AddScoped<IActiveVehicleRouteRepository, ActiveVehicleRouteRepository>();
        services.AddScoped<ActiveVehicleRouteAppService>();
        services.AddScoped<IRouteHistoryRepository, RouteHistoryRepository>();
        services.AddScoped<RouteHistoryAppService>();
        services.AddScoped<IOrderAssignmentRepository, OrderAssignmentRepository>();
        services.AddScoped<OrderAssignmentAppService>();
        services.AddScoped<IDriverVehicleAssignmentRepository, DriverVehicleAssignmentRepository>();
        services.AddScoped<DriverVehicleAssignmentAppService>();

        return services;
    }
}
