using Hiberus.Industria.Vektor.Domain.ActiveVehicleRoute;
using Hiberus.Industria.Vektor.Domain.Driver;
using Hiberus.Industria.Vektor.Domain.DriverVehicleAssignment;
using Hiberus.Industria.Vektor.Domain.Order;
using Hiberus.Industria.Vektor.Domain.OrderAssignment;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VektorDbContext).Assembly);
    }
}
