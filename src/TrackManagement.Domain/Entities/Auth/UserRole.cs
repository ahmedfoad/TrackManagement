using TrackManagement.Domain.Entities.Abstracts;

namespace TrackManagement.Domain.Entities.Auth;

public sealed class UserRole : BaseEntity<int>
{
    public int UserId { get; init; }
    public int RoleId { get; init; }

    public User User { get; init; } = null!;
    public Role Role { get; init; } = null!;
}