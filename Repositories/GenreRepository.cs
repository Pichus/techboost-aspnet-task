using techboost_aspnet.Data;
using techboost_aspnet.Entities;
using techboost_aspnet.Exceptions;

namespace techboost_aspnet.Repositories;

public class GenreRepository
{
    private readonly MusicCollectionDbContext _context;

    public GenreRepository(MusicCollectionDbContext context)
    {
        _context = context;
    }

    public bool GenreExists(string name)
    {
        return _context.Genres.Any(genre => genre.Name == name);
    }
    
    public void CreateGenre(string name)
    {
        bool genreExists = GenreExists(name);

        if (genreExists)
        {
            throw new EntityAlreadyExistsException(name);
        }

        _context.Add(new Genre() { Name = name });
        _context.SaveChanges();
    }
    
    public ICollection<Genre> GetAllGenres()
    {
        return _context.Genres.ToList();
    }

    public Genre GetGenreByName(string name)
    {
        var result = _context.Genres.FirstOrDefault(genre => genre.Name == name);

        if (result is null)
        {
            throw new EntityNotFoundException(name);
        }

        return result;
    }

    public ICollection<Genre> GetGenresByNames(string[] names)
    {
        var result = new List<Genre>();
        
        foreach (var name in names)
        {
            try
            {
                result.Add(GetGenreByName(name));
            }
            catch (EntityNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        return result;
    }
}