using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TrackManagement.Infrastructure;

public class TrackManagementReadDbContext : DbContext
{
    public TrackManagementReadDbContext(
        DbContextOptions<TrackManagementReadDbContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior =
            QueryTrackingBehavior.NoTracking;
    }

}