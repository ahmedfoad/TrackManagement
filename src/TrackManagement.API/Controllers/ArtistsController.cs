using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackManagement.Application.Features.Artists;

namespace TrackManagement.API.Controllers;

[ApiController]
[Authorize]
[Route("api/artists")]
public sealed class ArtistsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateArtist(
        [FromBody] CreateArtistDto artist,
        CancellationToken ct)
    {
        var result = await mediator.Send(new CreateArtistCommand(artist), ct);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<ArtistDto>>> GetArtists(CancellationToken ct)
    {
        var result = await mediator.Send(new ListArtistsQuery(), ct);
        return Ok(result);
    }
}
