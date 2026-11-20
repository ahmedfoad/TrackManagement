using TrackManagement.Domain.Entities.Abstracts;

namespace TrackManagement.Domain.Entities.Auth;

public sealed class User : BaseEntity<int>
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public ICollection<UserRole> UserRoles { get; init; } = [];
}
