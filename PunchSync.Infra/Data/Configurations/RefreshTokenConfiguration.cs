using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PunchSync.Domain.Entities;

namespace PunchSync.Infra.Data.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");

        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Token).IsRequired();
        builder.Property(rt => rt.ExpiresAt).IsRequired();

        builder.HasIndex(rt => rt.Token);
        builder.HasIndex(rt => rt.UserId);

        builder.Ignore(rt => rt.DomainEvents);
    }
}
