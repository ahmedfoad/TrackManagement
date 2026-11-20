using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackManagement.Domain.Entities;
using TrackManagement.Infrastructure.Configurations.Base;

namespace TrackManagement.Infrastructure.Configurations;

public class ArtistConfiguration : BaseEntityConfiguration<Artist, int>
{
    public override void Configure(EntityTypeBuilder<Artist> builder)
    {
        base.Configure(builder);

        // Properties
        builder.Property(x => x.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.County)
           .HasMaxLength(256)
           .IsRequired();

    

        // Navigation
        builder.HasMany(x => x.Tracks)
            .WithOne(x => x.Artist)
            .HasForeignKey(x => x.ArtistId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class TrackDistributionConfiguration : BaseEntityConfiguration<TrackDistribution, int>
{
    public override void Configure(EntityTypeBuilder<TrackDistribution> builder)
    {
        base.Configure(builder);

        // Navigation
        builder.HasOne(x => x.Track)
            .WithMany(x => x.TrackDistributions)
            .HasForeignKey(x => x.TrackId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Dsp)
            .WithMany(x => x.TrackDistributions)
            .HasForeignKey(x => x.DspId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

