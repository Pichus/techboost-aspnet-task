using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using techboost_aspnet.Data;
using techboost_aspnet.Dto;
using techboost_aspnet.Entities;
using techboost_aspnet.Exceptions;

namespace techboost_aspnet.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArtistController : ControllerBase
{
    private readonly MusicCollectionDbContext _context;

    public ArtistController(MusicCollectionDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Artist>>> GetArtists()
    {
        return await _context.Artists.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Artist>> GetArtist(int id)
    {
        var artist = await _context.Artists.FindAsync(id);

        if (artist == null) return NotFound();

        return artist;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutArtist(int id, ArtistDto artistDto)
    {
        Artist artist;
        try
        {
            artist = await DtoToEntity(artistDto);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }

        _context.Entry(artist).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ArtistExists(id)) return NotFound();

            throw;
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Artist>> PostArtist(ArtistDto artistDto)
    {
        Artist artist;
        try
        {
            artist = await DtoToEntity(artistDto);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }

        _context.Artists.Add(artist);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetArtist", new { id = artist.Id }, artist);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArtist(int id)
    {
        var artist = await _context.Artists.FindAsync(id);
        if (artist == null) return NotFound();

        _context.Artists.Remove(artist);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ArtistExists(int id)
    {
        return _context.Artists.Any(e => e.Id == id);
    }

    private async Task<Artist> DtoToEntity(ArtistDto artistDto)
    {
        var genres = new List<Genre>();

        foreach (var genreName in artistDto.GenreNames)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(genre1 => genre1.Name == genreName);

            if (genre is null) throw new EntityNotFoundException($"genre with name {genreName} not found");

            genres.Add(genre);
        }

        var artist = new Artist
        {
            Name = artistDto.Name,
            Country = artistDto.Country,
            YearsActiveStart = artistDto.YearsActiveStart,
            YearsActiveEnd = artistDto.YearsActiveEnd,
            Biography = artistDto.Biography,
            Genres = genres
        };

        return artist;
    }
}