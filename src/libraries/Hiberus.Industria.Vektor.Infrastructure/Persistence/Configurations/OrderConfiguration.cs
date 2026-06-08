using Hiberus.Industria.Vektor.Domain.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hiberus.Industria.Vektor.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : BaseConfiguration<Order>
{
    public override void Configure(EntityTypeBuilder<Order> builder)
    {
        base.Configure(builder);

        builder.ToTable("orders");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasColumnName("id");

        builder.HasIndex(o => o.TenantId);
        builder.HasIndex(o => o.Status);

        builder.Property(o => o.TenantId).HasColumnName("tenant_id");
        builder.Property(o => o.ExternalOrderId).HasColumnName("external_order_id");
        builder.Property(o => o.Label).HasColumnName("label").IsRequired();
        builder.Property(o => o.Description).HasColumnName("description");
        builder.Property(o => o.Latitude).HasColumnName("latitude");
        builder.Property(o => o.Longitude).HasColumnName("longitude");
        builder.Property(o => o.CustomerName).HasColumnName("customer_name");
        builder.Property(o => o.CustomerPhone).HasColumnName("customer_phone");

        builder.Property(o => o.Status).HasColumnName("status").HasConversion<string>();

        builder
            .HasOne(o => o.Tenant)
            .WithMany(t => t.Orders)
            .HasForeignKey(o => o.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
