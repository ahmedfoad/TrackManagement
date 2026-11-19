using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace TrackManagement.Infrastructure;

public class TrackManagementDbContext : DbContext
{
    public TrackManagementDbContext(
        DbContextOptions<TrackManagementDbContext> options)
        : base(options)
    {
    }

}
