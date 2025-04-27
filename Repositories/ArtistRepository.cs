using techboost_aspnet.Data;
using techboost_aspnet.Entities;
using techboost_aspnet.Exceptions;

namespace techboost_aspnet.Repositories;

public class ArtistRepository
{
    private readonly MusicCollectionDbContext _context;

    public ArtistRepository(MusicCollectionDbContext context)
    {
        _context = context;
    }

    public void CreateArtist(string name, string country, int yearsActiveStart, int yearsActiveEnd, string biography,
        ICollection<Genre>? genres = null)
    {
        bool artistExists = _context.Artists.Any(artist => artist.Name == name);

        if (artistExists)
        {
            throw new EntityAlreadyExistsException(name);
        }

        var artistToAdd = new Artist
        {
            Id = 0,
            Name = name,
            Country = country,
            YearsActiveStart = yearsActiveStart,
            YearsActiveEnd = yearsActiveEnd,
            Biography = biography
        };

        if (genres is not null)
        {
            artistToAdd.Genres = genres;
        }

        _context.Artists.Add(artistToAdd);
        _context.SaveChanges();
    }

    public Artist GetArtistByName(string name)
    {
        var result = _context.Artists.FirstOrDefault(artist => artist.Name == name);

        if (result is null)
        {
            throw new EntityNotFoundException(name);
        }

        return result;
    }

    public ICollection<Artist> GetArtistsByNames(string[] names)
    {
        var result = new List<Artist>();

        foreach (var name in names)
        {
            try
            {
                result.Add(GetArtistByName(name));
            }
            catch (EntityNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        return result;
    }

    public ICollection<Artist> GetAllArtists()
    {
        return _context.Artists.ToList();
    }
}