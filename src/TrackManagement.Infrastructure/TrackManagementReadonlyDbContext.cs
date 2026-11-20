using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TrackManagement.Domain.Entities;

namespace TrackManagement.Infrastructure;

public class TrackManagementReadonlyDbContext : DbContext
{
    public TrackManagementReadonlyDbContext(
        DbContextOptions<TrackManagementReadonlyDbContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior =
            QueryTrackingBehavior.NoTracking;
    }

    public DbSet<Artist> Artists { get; set; }
    public DbSet<Track> Tracks { get; set; }
    public DbSet<Dsp> Dsps { get; set; }
    public DbSet<TrackDistribution> TrackDistributions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrackManagementReadonlyDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}