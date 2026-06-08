using Hiberus.Industria.Vektor.Domain.Vehicle;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hiberus.Industria.Vektor.Infrastructure.Persistence.Configurations;

public class VehicleConfiguration : BaseConfiguration<Vehicle>
{
    public override void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        base.Configure(builder);

        builder.ToTable("vehicles");
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id).HasColumnName("id");

        builder.HasIndex(v => v.DeviceImei).IsUnique();
        builder.HasIndex(v => v.TenantId);

        builder.Property(v => v.Label).HasColumnName("label").IsRequired();

        builder.Property(v => v.LicensePlate).HasColumnName("license_plate");
        builder.Property(v => v.Brand).HasColumnName("brand");
        builder.Property(v => v.Model).HasColumnName("model");
        builder.Property(v => v.Year).HasColumnName("year");
        builder.Property(v => v.TenantId).HasColumnName("tenant_id");
        builder.Property(v => v.DeviceImei).HasColumnName("device_imei");
        builder.Property(v => v.FuelLevel).HasColumnName("fuel_level");
        builder.Property(v => v.BatteryLevel).HasColumnName("battery_level");
        builder.Property(v => v.EngineTemp).HasColumnName("engine_temp");
        builder.Property(v => v.Rpm).HasColumnName("rpm");
        builder.Property(v => v.Speed).HasColumnName("speed");
        builder.Property(v => v.Odometer).HasColumnName("odometer");
        builder.Property(v => v.DtcCodes).HasColumnName("dtc_codes");
        builder.Property(v => v.Latitude).HasColumnName("latitude");
        builder.Property(v => v.Longitude).HasColumnName("longitude");
        builder.Property(v => v.LastGpsUpdate).HasColumnName("last_gps_update");

        builder.Property(v => v.Type).HasColumnName("type").HasConversion<string>();

        builder.Property(v => v.Status).HasColumnName("status").HasConversion<string>();

        builder
            .HasOne(v => v.Tenant)
            .WithMany(t => t.Vehicles)
            .HasForeignKey(v => v.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
