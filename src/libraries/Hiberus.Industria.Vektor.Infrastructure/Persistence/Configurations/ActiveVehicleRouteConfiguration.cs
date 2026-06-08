using Hiberus.Industria.Vektor.Domain.ActiveVehicleRoute;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hiberus.Industria.Vektor.Infrastructure.Persistence.Configurations;

public class ActiveVehicleRouteConfiguration : BaseConfiguration<ActiveVehicleRoute>
{
    public override void Configure(EntityTypeBuilder<ActiveVehicleRoute> builder)
    {
        base.Configure(builder);

        builder.ToTable("active_vehicle_routes");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("id");

        builder.HasIndex(r => r.VehicleId).IsUnique();
        builder.HasIndex(r => r.TenantId);

        builder.Property(r => r.TenantId).HasColumnName("tenant_id");
        builder.Property(r => r.VehicleId).HasColumnName("vehicle_id");
        builder.Property(r => r.RoutePayload).HasColumnName("route_payload").IsRequired();
        builder.Property(r => r.AssociatedOrderIds).HasColumnName("associated_order_ids");
        builder.Property(r => r.StartedAt).HasColumnName("started_at");

        builder
            .HasOne(r => r.Tenant)
            .WithMany()
            .HasForeignKey(r => r.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
