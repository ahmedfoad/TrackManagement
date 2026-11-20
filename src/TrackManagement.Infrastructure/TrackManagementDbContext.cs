using Microsoft.EntityFrameworkCore;
using TrackManagement.Domain.Entities;

namespace TrackManagement.Infrastructure;

public class TrackManagementDbContext : DbContext
{
    public TrackManagementDbContext(
        DbContextOptions<TrackManagementDbContext> options)
        : base(options)
    {
    }

    public DbSet<Artist> Artists { get; set; }
    public DbSet<Track> Tracks { get; set; }
    public DbSet<Dsp> Dsps { get; set; }
    public DbSet<TrackDistribution> TrackDistributions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrackManagementDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
