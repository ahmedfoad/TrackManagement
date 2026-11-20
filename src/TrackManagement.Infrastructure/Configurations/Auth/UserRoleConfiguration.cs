using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackManagement.Domain.Entities.Auth;
using TrackManagement.Infrastructure.Configurations.Base;

namespace TrackManagement.Infrastructure.Configurations;

public class UserRoleConfiguration : BaseEntityConfiguration<UserRole, int>
{
    public override void Configure(EntityTypeBuilder<UserRole> builder)
    {
        base.Configure(builder);


        // Navigation
        builder.HasOne(x => x.User)
            .WithMany(x=>x.UserRoles)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Role)
            .WithMany(x => x.UserRoles)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}