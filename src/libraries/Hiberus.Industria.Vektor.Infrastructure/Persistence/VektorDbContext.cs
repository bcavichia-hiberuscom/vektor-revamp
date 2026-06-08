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
        base.OnModelCreating(modelBuilder);

        // Tenant
        modelBuilder.Entity<Tenant>(e =>
        {
            e.HasIndex(t => t.Slug).IsUnique();
        });

        // User
        modelBuilder.Entity<User>(e =>
        {
            e.HasIndex(u => new { u.Email, u.TenantId }).IsUnique();
            e.HasOne(u => u.Tenant)
                .WithMany(t => t.Users)
                .HasForeignKey(u => u.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Vehicle
        modelBuilder.Entity<Vehicle>(e =>
        {
            e.HasIndex(v => v.DeviceImei).IsUnique();
            e.HasIndex(v => v.TenantId);
            e.Property(v => v.Type).HasConversion<string>();
            e.HasOne(v => v.Tenant)
                .WithMany(t => t.Vehicles)
                .HasForeignKey(v => v.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Driver
        modelBuilder.Entity<Driver>(e =>
        {
            e.HasIndex(d => d.TenantId);
            e.Property(d => d.LicenseType).HasConversion<string>();
            e.HasOne(d => d.Tenant)
                .WithMany(t => t.Drivers)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // DriverVehicleAssignment
        modelBuilder.Entity<DriverVehicleAssignment>(e =>
        {
            e.HasIndex(a => new { a.DriverId, a.AssignedAt });
            e.HasIndex(a => new { a.VehicleId, a.AssignedAt });
            e.HasOne(a => a.Driver)
                .WithMany(d => d.VehicleAssignments)
                .HasForeignKey(a => a.DriverId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(a => a.Vehicle)
                .WithMany(v => v.Assignments)
                .HasForeignKey(a => a.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Order
        modelBuilder.Entity<Order>(e =>
        {
            e.HasIndex(o => o.TenantId);
            e.HasIndex(o => o.Status);
            e.Property(o => o.Status).HasConversion<string>();
            e.HasOne(o => o.Tenant)
                .WithMany(t => t.Orders)
                .HasForeignKey(o => o.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // OrderAssignment
        modelBuilder.Entity<OrderAssignment>(e =>
        {
            e.HasIndex(a => a.OrderId);
            e.HasIndex(a => a.VehicleId);
            e.HasIndex(a => a.Status);
            e.Property(a => a.Status).HasConversion<string>();
            e.HasOne(a => a.Order)
                .WithMany(o => o.Assignments)
                .HasForeignKey(a => a.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(a => a.Vehicle)
                .WithMany(v => v.OrderAssignments)
                .HasForeignKey(a => a.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ActiveVehicleRoute
        modelBuilder.Entity<ActiveVehicleRoute>(e =>
        {
            e.HasIndex(r => r.VehicleId).IsUnique();
            e.HasIndex(r => r.TenantId);
            e.HasOne(r => r.Tenant)
                .WithMany()
                .HasForeignKey(r => r.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
