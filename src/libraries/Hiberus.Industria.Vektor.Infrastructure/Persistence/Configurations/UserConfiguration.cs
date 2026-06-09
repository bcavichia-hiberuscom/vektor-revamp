using Hiberus.Industria.Vektor.Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hiberus.Industria.Vektor.Infrastructure.Persistence.Configurations;

public class UserConfiguration : BaseConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder); // covers auditable + soft deletes (ISoftDeletable)

        builder.ToTable("users");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnName("id");

        builder.HasIndex(u => new { u.Email, u.TenantId }).IsUnique();
        builder.HasIndex(u => u.TenantId);

        builder.Property(u => u.Email).HasColumnName("email").IsRequired();
        builder.Property(u => u.Name).HasColumnName("name");
        builder.Property(u => u.Role).HasColumnName("role").IsRequired().HasConversion<string>(); // stores enum as string in DB for better readability
        builder.Property(u => u.TenantId).HasColumnName("tenant_id");

        // The relationship is defined in TenantConfiguration (HasMany/WithOne),
        // here we only configure the FK on the child side
        builder
            .HasOne(u => u.Tenant)
            .WithMany(t => t.Users)
            .HasForeignKey(u => u.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
