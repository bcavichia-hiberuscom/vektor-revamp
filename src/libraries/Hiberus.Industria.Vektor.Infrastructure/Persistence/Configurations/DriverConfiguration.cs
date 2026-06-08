using Hiberus.Industria.Vektor.Domain.Driver;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hiberus.Industria.Vektor.Infrastructure.Persistence.Configurations;

public class DriverConfiguration : BaseConfiguration<Driver>
{
    public override void Configure(EntityTypeBuilder<Driver> builder)
    {
        base.Configure(builder);

        builder.ToTable("drivers");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasColumnName("id");

        builder.HasIndex(d => d.TenantId);

        builder.Property(d => d.TenantId).HasColumnName("tenant_id");
        builder.Property(d => d.Name).HasColumnName("name").IsRequired();
        builder.Property(d => d.PhoneNumber).HasColumnName("phone_number");
        builder.Property(d => d.LicenseNumber).HasColumnName("license_number");
        builder.Property(d => d.LicenseExpiryDate).HasColumnName("license_expiry_date");
        builder.Property(d => d.IsAvailable).HasColumnName("is_available");
        builder.Property(d => d.ImageUrl).HasColumnName("image_url");
        builder.Property(d => d.WorkdayStartTime).HasColumnName("workday_start_time");
        builder.Property(d => d.WorkdayEndTime).HasColumnName("workday_end_time");
        builder.Property(d => d.Timezone).HasColumnName("timezone");

        builder.Property(d => d.LicenseType).HasColumnName("license_type").HasConversion<string>();

        builder
            .HasOne(d => d.Tenant)
            .WithMany(t => t.Drivers)
            .HasForeignKey(d => d.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
