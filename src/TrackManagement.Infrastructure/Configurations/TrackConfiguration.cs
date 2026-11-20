using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackManagement.Domain.Entities;
using TrackManagement.Infrastructure.Configurations.Base;

namespace TrackManagement.Infrastructure.Configurations;

public class TrackConfiguration : BaseEntityConfiguration<Track, int>
{
    public override void Configure(EntityTypeBuilder<Track> builder)
    {
        base.Configure(builder);

        // Properties
        builder.Property(x => x.Title)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.ISrc)
            .HasMaxLength(2048)
            .IsRequired();
        builder.HasIndex(x => x.ISrc).IsUnique();

        builder.Property(x => x.Genre)
           .HasMaxLength(2048);
    }
}

