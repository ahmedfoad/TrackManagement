using TrackManagement.Domain.Entities.Abstracts;
using TrackManagement.Domain.Enums;

namespace TrackManagement.Domain.Entities;

public class TrackDistribution : BaseEntity<int>
{
    public int TrackId { get; set; }
    public Track Track { get; set; }

    public int DspId { get; set; }
    public Dsp Dsp { get; set; }

    public DateTime SubmittedAt { get; set; }
    public DistributionStatus DistributionStatus { get; set; } = DistributionStatus.Pending;

}
