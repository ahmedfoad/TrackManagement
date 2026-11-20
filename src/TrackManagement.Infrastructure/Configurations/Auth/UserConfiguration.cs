using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackManagement.Domain.Entities.Auth;
using TrackManagement.Infrastructure.Configurations.Base;

namespace TrackManagement.Infrastructure.Configurations;

public class UserConfiguration : BaseEntityConfiguration<User, int>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.Property(u => u.Username).HasMaxLength(100).IsRequired();
        builder.Property(u => u.Email).HasMaxLength(255).IsRequired();
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.CreatedAt).IsRequired();

        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.Username).IsUnique();
    }
}
