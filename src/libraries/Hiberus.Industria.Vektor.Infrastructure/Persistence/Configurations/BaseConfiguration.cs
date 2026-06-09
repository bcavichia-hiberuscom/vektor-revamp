using Hiberus.Industria.Vektor.Domain.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hiberus.Industria.Vektor.Infrastructure.Persistence.Configurations;

public abstract class BaseConfiguration<T> : IEntityTypeConfiguration<T>
    where T : class, IAuditable
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder
            .Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder
            .Property(e => e.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp with time zone");

        builder.Property(e => e.CreatedBy).HasColumnName("created_by").IsRequired();

        builder.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        if (typeof(ISoftDeletable).IsAssignableFrom(typeof(T)))
        {
            builder.Property("IsActive").HasColumnName("is_active").IsRequired();
            builder
                .Property("DeletedAt")
                .HasColumnName("deleted_at")
                .HasColumnType("timestamp with time zone");
            builder.Property("DeletedBy").HasColumnName("deleted_by");
        }
    }
}
