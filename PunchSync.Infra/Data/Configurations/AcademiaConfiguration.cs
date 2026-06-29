using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PunchSync.Domain.Entities;

namespace PunchSync.Infra.Data.Configurations;

public class AcademiaConfiguration : IEntityTypeConfiguration<Academia>
{
    public void Configure(EntityTypeBuilder<Academia> builder)
    {
        builder.ToTable("academias");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name).HasMaxLength(150).IsRequired();
        builder.Property(a => a.Slug).HasMaxLength(80).IsRequired();
        builder.Property(a => a.LogoUrl).HasMaxLength(500);
        builder.Property(a => a.Plan).HasConversion<int>();
        builder.Property(a => a.IsActive).HasDefaultValue(true);

        builder.HasIndex(a => a.Slug).IsUnique();

        builder.Ignore(a => a.DomainEvents);
    }
}
