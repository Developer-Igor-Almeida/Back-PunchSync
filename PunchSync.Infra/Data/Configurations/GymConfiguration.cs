using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PunchSync.Domain.Entities;

namespace PunchSync.Infra.Data.Configurations;

public class GymConfiguration : IEntityTypeConfiguration<Gym>
{
    public void Configure(EntityTypeBuilder<Gym> builder)
    {
        builder.ToTable("gyms");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Name).HasMaxLength(150).IsRequired();
        builder.Property(g => g.Slug).HasMaxLength(80).IsRequired();
        builder.Property(g => g.LogoUrl).HasMaxLength(500);
        builder.Property(g => g.Plan).HasConversion<int>();
        builder.Property(g => g.IsActive).HasDefaultValue(true);

        builder.HasIndex(g => g.Slug).IsUnique();

        builder.Ignore(g => g.DomainEvents);
    }
}
