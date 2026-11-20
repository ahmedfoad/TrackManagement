using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackManagement.Application.Features.Tracks;
using TrackManagement.Domain.Enums;

namespace TrackManagement.API.Controllers;

[ApiController]
[Authorize]
[Route("api/tracks")]
public sealed class TracksController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateTrack(
        [FromBody] CreateTrackDto track,
        CancellationToken ct)
    {
        try
        {
            var result = await mediator.Send(new CreateTrackCommand(track), ct);
            return CreatedAtAction(nameof(GetTrackById), new { id = result.Id }, result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<TrackDto>>> GetTracks(
        [FromQuery] int? artistId,
        [FromQuery] string? genre,
        [FromQuery] TrackStatus? status,
        CancellationToken ct)
    {
        var result = await mediator.Send(new ListTracksQuery(artistId, genre, status), ct);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TrackDetailsDto>> GetTrackById(int id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetTrackDetailsQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("{id:int}/distribute")]
    public async Task<IActionResult> DistributeTrack(
        int id,
        [FromBody] DistributeTrackDto distribution,
        CancellationToken ct)
    {
        try
        {
            var result = await mediator.Send(new DistributeTrackCommand(id, distribution), ct);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateTrackStatus(
        int id,
        [FromBody] UpdateTrackStatusDto update,
        CancellationToken ct)
    {
        try
        {
            var result = await mediator.Send(new UpdateTrackStatusCommand(id, update), ct);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
