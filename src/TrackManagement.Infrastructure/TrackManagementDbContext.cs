using Microsoft.EntityFrameworkCore;
using TrackManagement.Domain.Entities;
using TrackManagement.Domain.Entities.Auth;

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
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<TrackDistribution> TrackDistributions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrackManagementDbContext).Assembly);
        base.OnModelCreating(modelBuilder);

    }
}
