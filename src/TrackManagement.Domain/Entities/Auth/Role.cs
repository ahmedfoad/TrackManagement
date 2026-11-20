using TrackManagement.Domain.Entities.Abstracts;

namespace TrackManagement.Domain.Entities.Auth;

public sealed class Role : BaseEntity<int>
{
    public required string Name { get; set; }

    public ICollection<UserRole> UserRoles { get; init; } = [];
}
