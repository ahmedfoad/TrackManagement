using TrackManagement.Domain.Entities.Abstracts;

namespace TrackManagement.Domain.Entities;

public class Dsp : BaseEntity<int>
{
    public required string Name { get; set; }

    public ICollection<TrackDistribution> TrackDistributions { get; set; }
}
