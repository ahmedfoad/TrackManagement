using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackManagement.Domain.Entities;
using TrackManagement.Infrastructure.Configurations.Base;

namespace TrackManagement.Infrastructure.Configurations;

public class DspConfiguration : BaseEntityConfiguration<Dsp, int>
{
    public override void Configure(EntityTypeBuilder<Dsp> builder)
    {
        base.Configure(builder);

        // Properties
        builder.Property(x => x.Name)
            .HasMaxLength(512)
            .IsRequired();
    }
}

