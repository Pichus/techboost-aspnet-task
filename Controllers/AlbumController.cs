using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using techboost_aspnet.Data;
using techboost_aspnet.Dto;
using techboost_aspnet.Entities;
using techboost_aspnet.Exceptions;

namespace techboost_aspnet.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AlbumController : ControllerBase
{
    private readonly MusicCollectionDbContext _context;

    public AlbumController(MusicCollectionDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Album>>> GetAlbums()
    {
        return await _context.Albums.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Album>> GetAlbum(int id)
    {
        var album = await _context.Albums.FindAsync(id);

        if (album == null) return NotFound();

        return album;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAlbum(int id, AlbumDto albumDto)
    {
        Album album;
        try
        {
            album = await DtoToEntity(albumDto);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }

        _context.Entry(album).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AlbumExists(id)) return NotFound();

            throw;
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Album>> PostAlbum(AlbumDto albumDto)
    {
        Album album;
        try
        {
            album = await DtoToEntity(albumDto);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }

        _context.Albums.Add(album);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetAlbum", new { id = album.Id }, album);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAlbum(int id)
    {
        var album = await _context.Albums.FindAsync(id);
        if (album == null) return NotFound();

        _context.Albums.Remove(album);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AlbumExists(int id)
    {
        return _context.Albums.Any(e => e.Id == id);
    }

    private async Task<Album> DtoToEntity(AlbumDto albumDto)
    {
        ValidateAlbumType(albumDto.Type);
        
        var genres = new List<Genre>();

        foreach (var genreName in albumDto.GenreNames)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(genre1 => genre1.Name == genreName);

            if (genre is null) throw new EntityNotFoundException($"genre with name {genreName} not found");

            genres.Add(genre);
        }

        var artists = new List<Artist>();

        foreach (var artistName in albumDto.ArtistNames)
        {
            var artist = await _context.Artists.FirstOrDefaultAsync(artist1 => artist1.Name == artistName);

            if (artist is null) throw new EntityNotFoundException($"artist with name {artistName} not found");

            artists.Add(artist);
        }

        var album = new Album
        {
            Title = albumDto.Title,
            ReleaseYear = albumDto.ReleaseYear,
            NumberOfTracks = albumDto.NumberOfTracks,
            Label = albumDto.Label,
            Type = albumDto.Type,
            Genres = genres,
            Artists = artists
        };

        return album;
    }
    
    private void ValidateAlbumType(string type)
    {
        string[] types = ["CD", "Vinyl", "Digital"];
        if (!types.Contains(type)) throw new ArgumentException("type should be either 'CD', 'Vinyl' or 'Digital'");
    }
}