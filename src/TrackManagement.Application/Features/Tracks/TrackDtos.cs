using TrackManagement.Domain.Enums;

namespace TrackManagement.Application.Features.Tracks;

public sealed record CreateTrackDto(
    string Title,
    int ArtistId,
    string ISrc,
    DateTime ReleaseDate,
    string Genre);

public sealed record TrackDto(
    int Id,
    string Title,
    int ArtistId,
    string ArtistName,
    string ISrc,
    DateTime ReleaseDate,
    string Genre,
    TrackStatus Status);

public sealed record TrackDistributionDto(
    int DspId,
    string DspName,
    DistributionStatus Status,
    DateTime SubmittedAt);

public sealed record TrackDetailsDto(
    int Id,
    string Title,
    int ArtistId,
    string ArtistName,
    string ISrc,
    DateTime ReleaseDate,
    string Genre,
    TrackStatus Status,
    IReadOnlyCollection<TrackDistributionDto> Distributions);

public sealed record DistributeTrackDto(IReadOnlyCollection<int> DspIds);

public sealed record UpdateTrackStatusDto(TrackStatus Status);
