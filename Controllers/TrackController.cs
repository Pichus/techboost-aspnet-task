using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using techboost_aspnet.Data;
using techboost_aspnet.Dto;
using techboost_aspnet.Entities;
using techboost_aspnet.Exceptions;

namespace techboost_aspnet.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TrackController : ControllerBase
{
    private readonly MusicCollectionDbContext _context;

    public TrackController(MusicCollectionDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Track>>> GetTracks()
    {
        return await _context.Tracks.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Track>> GetTrack(int id)
    {
        var track = await _context.Tracks.FindAsync(id);

        if (track == null) return NotFound();

        return track;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTrack(int id, TrackDto trackDto)
    {
        Track track;

        try
        {
            track = await DtoToEntity(trackDto);
        }
        catch (FormatException e)
        {
            return BadRequest(e.Message);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
        
        _context.Entry(track).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TrackExists(id)) return NotFound();

            throw;
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Track>> PostTrack(TrackDto trackDto)
    {
        Track track;

        try
        {
            track = await DtoToEntity(trackDto);
        }
        catch (FormatException e)
        {
            return BadRequest(e.Message);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
        
        _context.Tracks.Add(track);
        var album = await _context.Albums.FindAsync(track.AlbumId);
        if (album != null) album.NumberOfTracks++;
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTrack", new { id = track.Id }, track);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrack(int id)
    {
        var track = await _context.Tracks.FindAsync(id);
        if (track == null) return NotFound();

        _context.Tracks.Remove(track);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TrackExists(int id)
    {
        return _context.Tracks.Any(e => e.Id == id);
    }

    private async Task<Track> DtoToEntity(TrackDto trackDto)
    {
        TimeSpan duration;
        bool parseDurationResult = TimeSpan.TryParse(trackDto.Duration, out duration);

        if (!parseDurationResult)
        {
            throw new FormatException();
        }
        
        var artists = new List<Artist>();

        foreach (var artistId in trackDto.ArtistIds)
        {
            var artist = await _context.Artists.FindAsync(artistId);

            if (artist is null)
            {
                throw new EntityNotFoundException($"artist with id {artistId} not found");
            }
            
            artists.Add(artist);
        }
        
        var track = new Track
        {
            Title = trackDto.Title,
            Duration = duration,
            AlbumId = trackDto.AlbumId,
            ChartPosition = trackDto.ChartPosition,
            Lyricist = trackDto.Lyricist,
            Artists = artists
        };

        return track;
    }
}