using TrackManagement.Domain.Entities.Abstracts;

namespace TrackManagement.Domain.Entities;

public class Artist : BaseEntity<int>
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string County { get; set; }
    public ICollection<Track> Tracks { get; set; }
}

