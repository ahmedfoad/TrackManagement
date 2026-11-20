using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TrackManagement.Domain.Entities;
using TrackManagement.Domain.Enums;
using TrackManagement.Infrastructure;

namespace TrackManagement.Application.Features.Tracks;

public sealed record CreateTrackCommand(CreateTrackDto Track) : IRequest<TrackDto>;

public sealed class CreateTrackCommandValidator : AbstractValidator<CreateTrackCommand>
{
    public CreateTrackCommandValidator()
    {
        RuleFor(command => command.Track.Title)
            .NotEmpty()
            .MaximumLength(256);
        RuleFor(command => command.Track.ArtistId)
            .GreaterThan(0);
        RuleFor(command => command.Track.ISrc)
            .NotEmpty()
            .MaximumLength(2048);
        RuleFor(command => command.Track.ReleaseDate)
            .NotEmpty();
        RuleFor(command => command.Track.Genre)
            .NotEmpty()
            .MaximumLength(2048);
    }
}

public sealed class CreateTrackCommandHandler(TrackManagementDbContext db)
    : IRequestHandler<CreateTrackCommand, TrackDto>
{
    public async Task<TrackDto> Handle(CreateTrackCommand request, CancellationToken ct)
    {
        var artist = await db.Artists
            .SingleOrDefaultAsync(item => item.Id == request.Track.ArtistId, ct)
            ?? throw new KeyNotFoundException($"Artist {request.Track.ArtistId} was not found.");

        var track = new Track
        {
            Title = request.Track.Title.Trim(),
            ArtistId = artist.Id,
            Artist = artist,
            ISrc = request.Track.ISrc.Trim(),
            ReleasDate = request.Track.ReleaseDate,
            Genre = request.Track.Genre.Trim(),
            TrackStatus = TrackStatus.Draft,
            TrackDistributions = []
        };

        db.Tracks.Add(track);
        await db.SaveChangesAsync(ct);

        return new TrackDto(
            track.Id,
            track.Title,
            track.ArtistId,
            artist.Name,
            track.ISrc,
            track.ReleasDate,
            track.Genre,
            track.TrackStatus);
    }
}

public sealed record DistributeTrackCommand(int TrackId, DistributeTrackDto Distribution)
    : IRequest<TrackDetailsDto>;

public sealed class DistributeTrackCommandValidator : AbstractValidator<DistributeTrackCommand>
{
    public DistributeTrackCommandValidator()
    {
        RuleFor(command => command.TrackId)
            .GreaterThan(0);
        RuleFor(command => command.Distribution.DspIds)
            .Must(ids => ids is { Count: > 0 }
                && ids.All(id => id > 0)
                && ids.Distinct().Count() == ids.Count)
            .WithMessage("Provide one or more unique positive DSP IDs.");
    }
}

public sealed class DistributeTrackCommandHandler(TrackManagementDbContext db)
    : IRequestHandler<DistributeTrackCommand, TrackDetailsDto>
{
    public async Task<TrackDetailsDto> Handle(DistributeTrackCommand request, CancellationToken ct)
    {
        var track = await db.Tracks
            .Include(item => item.Artist)
            .Include(item => item.TrackDistributions)
                .ThenInclude(distribution => distribution.Dsp)
            .SingleOrDefaultAsync(item => item.Id == request.TrackId, ct)
            ?? throw new KeyNotFoundException($"Track {request.TrackId} was not found.");

        var dspIds = request.Distribution.DspIds.ToArray();
        var dsps = await db.Dsps
            .Where(dsp => dspIds.Contains(dsp.Id))
            .ToListAsync(ct);
        var foundDspIds = dsps.Select(dsp => dsp.Id).ToHashSet();
        var missingDspIds = dspIds.Where(id => !foundDspIds.Contains(id)).ToArray();
        if (missingDspIds.Length > 0)
        {
            throw new KeyNotFoundException($"DSP IDs not found: {string.Join(", ", missingDspIds)}.");
        }

        var existingDspIds = track.TrackDistributions
            .Select(distribution => distribution.DspId)
            .ToHashSet();
        var submittedAt = DateTime.UtcNow;
        foreach (var dsp in dsps.Where(dsp => !existingDspIds.Contains(dsp.Id)))
        {
            track.TrackDistributions.Add(new TrackDistribution
            {
                Track = track,
                Dsp = dsp,
                SubmittedAt = submittedAt,
                DistributionStatus = DistributionStatus.Pending
            });
        }

        track.TrackStatus = TrackStatus.Submitted;
        await db.SaveChangesAsync(ct);

        return new TrackDetailsDto(
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

public sealed record UpdateTrackStatusCommand(int TrackId, UpdateTrackStatusDto Update)
    : IRequest<TrackDto>;

public sealed class UpdateTrackStatusCommandValidator : AbstractValidator<UpdateTrackStatusCommand>
{
    public UpdateTrackStatusCommandValidator()
    {
        RuleFor(command => command.TrackId)
            .GreaterThan(0);
        RuleFor(command => command.Update.Status)
            .IsInEnum();
    }
}

public sealed class UpdateTrackStatusCommandHandler(TrackManagementDbContext db)
    : IRequestHandler<UpdateTrackStatusCommand, TrackDto>
{
    public async Task<TrackDto> Handle(UpdateTrackStatusCommand request, CancellationToken ct)
    {
        var track = await db.Tracks
            .Include(item => item.Artist)
            .SingleOrDefaultAsync(item => item.Id == request.TrackId, ct)
            ?? throw new KeyNotFoundException($"Track {request.TrackId} was not found.");

        track.TrackStatus = request.Update.Status;
        await db.SaveChangesAsync(ct);

        return new TrackDto(
            track.Id,
            track.Title,
            track.ArtistId,
            track.Artist.Name,
            track.ISrc,
            track.ReleasDate,
            track.Genre,
            track.TrackStatus);
    }
}
