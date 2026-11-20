using TrackManagement.Domain.Entities.Abstracts;
using TrackManagement.Domain.Enums;

namespace TrackManagement.Domain.Entities;

public class Track : BaseEntity<int>
{
    public required string Title { get; set; }
    public int ArtistId { get; set; }
    public Artist Artist { get; set; }
    public required string ISrc { get; set; } //unique identifier per track
    public DateTime ReleasDate { get; set; }
    public string Genre { get; set; } = default!;
    public TrackStatus TrackStatus { get; set; } = TrackStatus.Draft;

    public ICollection<TrackDistribution> TrackDistributions { get; set; }
}
