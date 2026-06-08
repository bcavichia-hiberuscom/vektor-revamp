using Hiberus.Industria.Vektor.Domain.Tenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hiberus.Industria.Vektor.Infrastructure.Persistence.Configurations;

public class TenantConfiguration : BaseConfiguration<Tenant>
{
    public override void Configure(EntityTypeBuilder<Tenant> builder)
    {
        base.Configure(builder);

        builder.ToTable("tenants");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasColumnName("id");

        builder.HasIndex(t => t.Slug).IsUnique();

        builder.Property(t => t.Name).HasColumnName("name").IsRequired();

        builder.Property(t => t.Slug).HasColumnName("slug").IsRequired();
    }
}
