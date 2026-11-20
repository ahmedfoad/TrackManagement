using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TrackManagement.Domain.Entities;
using TrackManagement.Infrastructure;

namespace TrackManagement.Application.Features.Artists;

public sealed record CreateArtistDto(string Name, string Email, string County);

public sealed record ArtistDto(int Id, string Name, string Email, string County);

public sealed record CreateArtistCommand(CreateArtistDto Artist) : IRequest<ArtistDto>;

public sealed class CreateArtistCommandValidator : AbstractValidator<CreateArtistCommand>
{
    public CreateArtistCommandValidator()
    {
        RuleFor(command => command.Artist.Name)
            .NotEmpty()
            .MaximumLength(256);
        RuleFor(command => command.Artist.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);
        RuleFor(command => command.Artist.County)
            .NotEmpty()
            .MaximumLength(256);
    }
}

public sealed class CreateArtistCommandHandler(TrackManagementDbContext db)
    : IRequestHandler<CreateArtistCommand, ArtistDto>
{
    public async Task<ArtistDto> Handle(CreateArtistCommand request, CancellationToken ct)
    {
        var artist = new Artist
        {
            Name = request.Artist.Name.Trim(),
            Email = request.Artist.Email.Trim(),
            County = request.Artist.County.Trim(),
            Tracks = []
        };

        db.Artists.Add(artist);
        await db.SaveChangesAsync(ct);

        return new ArtistDto(artist.Id, artist.Name, artist.Email, artist.County);
    }
}

public sealed record ListArtistsQuery : IRequest<IReadOnlyCollection<ArtistDto>>;

public sealed class ListArtistsQueryHandler(TrackManagementReadonlyDbContext db)
    : IRequestHandler<ListArtistsQuery, IReadOnlyCollection<ArtistDto>>
{
    public async Task<IReadOnlyCollection<ArtistDto>> Handle(ListArtistsQuery request, CancellationToken ct)
    {
        return await db.Artists
            .OrderBy(artist => artist.Name)
            .Select(artist => new ArtistDto(artist.Id, artist.Name, artist.Email, artist.County))
            .ToListAsync(ct);
    }
}
