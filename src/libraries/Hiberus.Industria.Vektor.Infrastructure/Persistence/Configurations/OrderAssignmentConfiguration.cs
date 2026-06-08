using Hiberus.Industria.Vektor.Domain.OrderAssignment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hiberus.Industria.Vektor.Infrastructure.Persistence.Configurations;

public class OrderAssignmentConfiguration : BaseConfiguration<OrderAssignment>
{
    public override void Configure(EntityTypeBuilder<OrderAssignment> builder)
    {
        base.Configure(builder);

        builder.ToTable("order_assignments");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id");

        builder.HasIndex(a => a.OrderId);
        builder.HasIndex(a => a.VehicleId);
        builder.HasIndex(a => a.Status);

        builder.Property(a => a.OrderId).HasColumnName("order_id");
        builder.Property(a => a.VehicleId).HasColumnName("vehicle_id");
        builder.Property(a => a.AssignedAt).HasColumnName("assigned_at");
        builder.Property(a => a.StartedAt).HasColumnName("started_at");
        builder.Property(a => a.CompletedAt).HasColumnName("completed_at");
        builder.Property(a => a.ActualArrival).HasColumnName("actual_arrival");
        builder.Property(a => a.FailureReason).HasColumnName("failure_reason");

        builder.Property(a => a.Status).HasColumnName("status").HasConversion<string>();

        builder
            .HasOne(a => a.Order)
            .WithMany(o => o.Assignments)
            .HasForeignKey(a => a.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(a => a.Vehicle)
            .WithMany(v => v.OrderAssignments)
            .HasForeignKey(a => a.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
