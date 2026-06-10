using Hiberus.Industria.Vektor.Domain.ActiveVehicleRoute;
using Hiberus.Industria.Vektor.Domain.Driver;
using Hiberus.Industria.Vektor.Domain.DriverVehicleAssignment;
using Hiberus.Industria.Vektor.Domain.Order;
using Hiberus.Industria.Vektor.Domain.OrderAssignment;
using Hiberus.Industria.Vektor.Domain.RouteHistory;
using Hiberus.Industria.Vektor.Domain.Tenant;
using Hiberus.Industria.Vektor.Domain.User;
using Hiberus.Industria.Vektor.Domain.Vehicle;
using Microsoft.EntityFrameworkCore;

namespace Hiberus.Industria.Vektor.Infrastructure.Persistence;

public class VektorDbContext : DbContext
{
    public VektorDbContext(DbContextOptions<VektorDbContext> options)
        : base(options) { }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Driver> Drivers => Set<Driver>();
    public DbSet<DriverVehicleAssignment> DriverVehicleAssignments =>
        Set<DriverVehicleAssignment>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderAssignment> OrderAssignments => Set<OrderAssignment>();
    public DbSet<ActiveVehicleRoute> ActiveVehicleRoutes => Set<ActiveVehicleRoute>();
    public DbSet<RouteHistory> RouteHistories => Set<RouteHistory>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        // Note: Lazy loading is disabled at the model level in OnModelCreating
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Disable lazy loading at the model level
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var navigation in entityType.GetNavigations())
            {
                navigation.SetLazyLoadingEnabled(false);
            }
        }

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VektorDbContext).Assembly);
    }
}
