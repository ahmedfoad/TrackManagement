using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TrackManagement.Domain.Entities;
using TrackManagement.Domain.Enums;

namespace TrackManagement.Infrastructure;

public static class DatabaseInitializer
{
    private static readonly ArtistSeed[] Artists =
    [
        new("maya.chen@example.test", "Maya Chen", "United States"),
        new("theo.rivera@example.test", "Theo Rivera", "Canada"),
        new("amara.okafor@example.test", "Amara Okafor", "United Kingdom")
    ];

    private static readonly TrackSeed[] Tracks =
    [
        new("maya.chen@example.test", "City Lights", "USAAA2600001", new DateTime(2026, 1, 16), "Pop", TrackStatus.Distributed),
        new("maya.chen@example.test", "Paper Planes", "USAAA2600002", new DateTime(2026, 2, 20), "Indie Pop", TrackStatus.Submitted),
        new("maya.chen@example.test", "After the Rain", "USAAA2600003", new DateTime(2026, 3, 6), "R&B", TrackStatus.Draft),
        new("theo.rivera@example.test", "Open Road", "CAAAA2600001", new DateTime(2026, 1, 30), "Folk", TrackStatus.Distributed),
        new("theo.rivera@example.test", "Midnight Run", "CAAAA2600002", new DateTime(2026, 2, 13), "Rock", TrackStatus.Submitted),
        new("theo.rivera@example.test", "Small Hours", "CAAAA2600003", new DateTime(2026, 3, 13), "Electronic", TrackStatus.Draft),
        new("amara.okafor@example.test", "Golden Hour", "GBBBB2600001", new DateTime(2026, 1, 23), "Afrobeats", TrackStatus.Distributed),
        new("amara.okafor@example.test", "Good Company", "GBBBB2600002", new DateTime(2026, 2, 27), "Soul", TrackStatus.Submitted),
        new("amara.okafor@example.test", "Home Again", "GBBBB2600003", new DateTime(2026, 3, 20), "Acoustic", TrackStatus.Draft)
    ];

    private static readonly string[] DspNames = ["Spotify", "Apple Music", "YouTube Music"];

    public static async Task InitializeDatabaseAsync(this IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TrackManagementDbContext>();

        await dbContext.Database.MigrateAsync();
        await SeedAsync(dbContext);
    }

    private static async Task SeedAsync(TrackManagementDbContext dbContext)
    {
        var artistEmails = Artists.Select(artist => artist.Email).ToArray();
        var existingArtistEmails = await dbContext.Artists
            .Where(artist => artistEmails.Contains(artist.Email))
            .Select(artist => artist.Email)
            .ToListAsync();
        var knownArtistEmails = new HashSet<string>(existingArtistEmails, StringComparer.OrdinalIgnoreCase);

        foreach (var artist in Artists.Where(artist => !knownArtistEmails.Contains(artist.Email)))
        {
            dbContext.Artists.Add(new Artist
            {
                Name = artist.Name,
                Email = artist.Email,
                County = artist.County,
                Tracks = []
            });
        }

        await dbContext.SaveChangesAsync();

        var trackIsrcs = Tracks.Select(track => track.Isrc).ToArray();
        var existingIsrcs = await dbContext.Tracks
            .Where(track => trackIsrcs.Contains(track.ISrc))
            .Select(track => track.ISrc)
            .ToListAsync();
        var knownIsrcs = new HashSet<string>(existingIsrcs, StringComparer.OrdinalIgnoreCase);

        var artistsByEmail = await dbContext.Artists
            .Where(artist => artistEmails.Contains(artist.Email))
            .ToDictionaryAsync(artist => artist.Email, StringComparer.OrdinalIgnoreCase);

        foreach (var track in Tracks.Where(track => !knownIsrcs.Contains(track.Isrc)))
        {
            dbContext.Tracks.Add(new Track
            {
                Title = track.Title,
                Artist = artistsByEmail[track.ArtistEmail],
                ISrc = track.Isrc,
                ReleasDate = track.ReleaseDate,
                Genre = track.Genre,
                TrackStatus = track.Status,
                TrackDistributions = []
            });
        }

        var existingDspNames = await dbContext.Dsps
            .Where(dsp => DspNames.Contains(dsp.Name))
            .Select(dsp => dsp.Name)
            .ToListAsync();
        var knownDspNames = new HashSet<string>(existingDspNames, StringComparer.OrdinalIgnoreCase);

        foreach (var dspName in DspNames.Where(name => !knownDspNames.Contains(name)))
        {
            dbContext.Dsps.Add(new Dsp
            {
                Name = dspName,
                TrackDistributions = []
            });
        }

        await dbContext.SaveChangesAsync();
    }

    private sealed record ArtistSeed(string Email, string Name, string County);

    private sealed record TrackSeed(
        string ArtistEmail,
        string Title,
        string Isrc,
        DateTime ReleaseDate,
        string Genre,
        TrackStatus Status);
}
