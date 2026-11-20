using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TrackManagement.Domain.Entities;
using TrackManagement.Domain.Enums;
using TrackManagement.Infrastructure;

namespace TrackManagement.Application.Features.Tracks;

public sealed record ListTracksQuery(int? ArtistId, string? Genre, TrackStatus? Status)
    : IRequest<IReadOnlyCollection<TrackDto>>;

public sealed class ListTracksQueryValidator : AbstractValidator<ListTracksQuery>
{
    public ListTracksQueryValidator()
    {
        RuleFor(query => query.ArtistId)
            .GreaterThan(0)
            .When(query => query.ArtistId.HasValue);
        RuleFor(query => query.Genre)
            .MaximumLength(2048);
        RuleFor(query => query.Status)
            .Must(status => !status.HasValue || Enum.IsDefined(status.Value));
    }
}

public sealed class ListTracksQueryHandler(TrackManagementReadonlyDbContext db)
    : IRequestHandler<ListTracksQuery, IReadOnlyCollection<TrackDto>>
{
    public async Task<IReadOnlyCollection<TrackDto>> Handle(ListTracksQuery request, CancellationToken ct)
    {
        var tracks = db.Tracks.AsQueryable();

        if (request.ArtistId.HasValue)
        {
            tracks = tracks.Where(track => track.ArtistId == request.ArtistId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Genre))
        {
            var genre = request.Genre.Trim();
            tracks = tracks.Where(track => track.Genre == genre);
        }

        if (request.Status.HasValue)
        {
            tracks = tracks.Where(track => track.TrackStatus == request.Status.Value);
        }

        return await tracks
            .OrderBy(track => track.Title)
            .Select(track => new TrackDto(
                track.Id,
                track.Title,
                track.ArtistId,
                track.Artist.Name,
                track.ISrc,
                track.ReleasDate,
                track.Genre,
                track.TrackStatus))
            .ToListAsync(ct);
    }
}

public sealed record GetTrackDetailsQuery(int Id) : IRequest<TrackDetailsDto?>;

public sealed class GetTrackDetailsQueryHandler(TrackManagementReadonlyDbContext db)
    : IRequestHandler<GetTrackDetailsQuery, TrackDetailsDto?>
{
    public async Task<TrackDetailsDto?> Handle(GetTrackDetailsQuery request, CancellationToken ct)
    {
        var track = await db.Tracks
            .Include(item => item.Artist)
            .Include(item => item.TrackDistributions)
                .ThenInclude(distribution => distribution.Dsp)
            .SingleOrDefaultAsync(item => item.Id == request.Id, ct);

        return track is null ? null : new TrackDetailsDto(
            track.Id,
            track.Title,
            track.ArtistId,
            track.Artist.Name,
            track.ISrc,
            track.ReleasDate,
            track.Genre,
            track.TrackStatus,
            track.TrackDistributions
                .OrderBy(distribution => distribution.Dsp.Name)
                .Select(distribution => new TrackDistributionDto(
                    distribution.DspId,
                    distribution.Dsp.Name,
                    distribution.DistributionStatus,
                    distribution.SubmittedAt))
                .ToArray());
    }
}
