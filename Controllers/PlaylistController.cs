using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using techboost_aspnet.Data;
using techboost_aspnet.Dto;
using techboost_aspnet.Entities;

namespace techboost_aspnet.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlaylistController : ControllerBase
{
    private readonly MusicCollectionDbContext _context;

    public PlaylistController(MusicCollectionDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Playlist>>> GetPlaylists()
    {
        return await _context.Playlists.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Playlist>> GetPlaylist(int id)
    {
        var playlist = await _context.Playlists.FindAsync(id);

        if (playlist == null) return NotFound();

        return playlist;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPlaylist(int id, PlaylistDto playlistDto)
    {
        var playlist = DtoToEntity(playlistDto);

        _context.Entry(playlist).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PlaylistExists(id)) return NotFound();

            throw;
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Playlist>> PostPlaylist(PlaylistDto playlistDto)
    {
        var playlist = DtoToEntity(playlistDto);

        _context.Playlists.Add(playlist);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetPlaylist", new { id = playlist.Id }, playlist);
    }

    [HttpPost("{playlistId}/{trackId}")]
    public async Task<ActionResult> AddTrackToPlaylist(int playlistId, int trackId)
    {
        var playlist = await _context.Playlists.FirstOrDefaultAsync(playlist1 => playlist1.Id == playlistId);
        var track = await _context.Tracks.FirstOrDefaultAsync(track1 => track1.Id == trackId);

        if (playlist is null || track is null) return NotFound();

        playlist.Tracks.Add(track);
        await _context.SaveChangesAsync();

        return CreatedAtAction("", playlistId, playlist);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlaylist(int id)
    {
        var playlist = await _context.Playlists.FindAsync(id);
        if (playlist == null) return NotFound();

        _context.Playlists.Remove(playlist);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PlaylistExists(int id)
    {
        return _context.Playlists.Any(e => e.Id == id);
    }

    private Playlist DtoToEntity(PlaylistDto playlistDto)
    {
        var playlist = new Playlist
        {
            Name = playlistDto.Name,
            UserId = playlistDto.UserId,
            CreatedAt = DateTime.UtcNow
        };

        return playlist;
    }
}