using Hiberus.Industria.Vektor.Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hiberus.Industria.Vektor.Infrastructure.Persistence.Configurations;

public class UserConfiguration : BaseConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.ToTable("users");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnName("id");

        builder.HasIndex(u => new { u.Email, u.TenantId }).IsUnique();
        builder.HasIndex(u => u.TenantId);

        builder.Property(u => u.Email).HasColumnName("email").IsRequired();

        builder.Property(u => u.Name).HasColumnName("name");

        builder.Property(u => u.Role).HasColumnName("role").IsRequired();

        builder.Property(u => u.IsActive).HasColumnName("is_active");

        builder.Property(u => u.TenantId).HasColumnName("tenant_id");

        builder
            .HasOne(u => u.Tenant)
            .WithMany(t => t.Users)
            .HasForeignKey(u => u.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
