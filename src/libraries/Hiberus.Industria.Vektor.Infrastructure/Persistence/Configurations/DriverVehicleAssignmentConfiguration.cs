using Hiberus.Industria.Vektor.Domain.DriverVehicleAssignment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hiberus.Industria.Vektor.Infrastructure.Persistence.Configurations;

public class DriverVehicleAssignmentConfiguration : BaseConfiguration<DriverVehicleAssignment>
{
    public override void Configure(EntityTypeBuilder<DriverVehicleAssignment> builder)
    {
        base.Configure(builder);

        builder.ToTable("driver_vehicle_assignments");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id");

        builder.HasIndex(a => new { a.DriverId, a.AssignedAt });
        builder.HasIndex(a => new { a.VehicleId, a.AssignedAt });

        builder.Property(a => a.DriverId).HasColumnName("driver_id");
        builder.Property(a => a.VehicleId).HasColumnName("vehicle_id");
        builder.Property(a => a.AssignedAt).HasColumnName("assigned_at");
        builder.Property(a => a.UnassignedAt).HasColumnName("unassigned_at");

        builder
            .HasOne(a => a.Driver)
            .WithMany(d => d.VehicleAssignments)
            .HasForeignKey(a => a.DriverId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(a => a.Vehicle)
            .WithMany(v => v.Assignments)
            .HasForeignKey(a => a.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
