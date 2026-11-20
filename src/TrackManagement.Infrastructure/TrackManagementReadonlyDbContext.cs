using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TrackManagement.Domain.Entities;
using TrackManagement.Domain.Entities.Auth;

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

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrackManagementReadonlyDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}