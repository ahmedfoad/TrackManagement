using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackManagement.Domain.Entities;
using TrackManagement.Domain.Entities.Auth;
using TrackManagement.Infrastructure.Configurations.Base;

namespace TrackManagement.Infrastructure.Configurations;

public class RoleConfiguration : BaseEntityConfiguration<Role, int>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);

        builder.Property(r => r.Name).HasMaxLength(50).IsRequired();
        builder.HasIndex(r => r.Name).IsUnique();
    }
}
