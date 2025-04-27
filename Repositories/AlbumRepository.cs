using techboost_aspnet.Data;
using techboost_aspnet.Entities;
using techboost_aspnet.Exceptions;

namespace techboost_aspnet.Repositories;

public class AlbumRepository
{
    private readonly MusicCollectionDbContext _context;

    public AlbumRepository(MusicCollectionDbContext context)
    {
        _context = context;
    }

    private void ValidateAlbumType(string type)
    {
        string[] types = ["CD", "Vinyl", "Digital"];
        if (!types.Contains(type))
        {
            throw new ArgumentException("type should be either 'CD', 'Vinyl' or 'Digital'");
        }
    }
    
    public void CreateAlbum(string title, int releaseYear, string label, string type,
        ICollection<Artist>? artists = null, ICollection<Genre>? genres = null)
    {
        ValidateAlbumType(type);

        bool albumExists = _context.Albums.Any(album => album.Title == title);

        if (albumExists)
        {
            throw new EntityAlreadyExistsException(title);
        }

        var albumToAdd = new Album()
        {
            Title = title,
            ReleaseYear = releaseYear,
            Label = label,
            Type = type,
        };

        if (artists is not null)
        {
            albumToAdd.Artists = artists;
        }

        if (genres is not null)
        {
            albumToAdd.Genres = genres;
        }

        _context.Albums.Add(albumToAdd);
        _context.SaveChanges();
    }

    public ICollection<Album> GetAlbums()
    {
        return _context.Albums.ToList();
    }

    public Album GetAlbumByTitle(string title)
    {
        var result = _context.Albums.FirstOrDefault(album => album.Title == title);

        if (result is null)
        {
            throw new EntityNotFoundException(title);
        }

        return result;
    }

    public void IncrementAlbumTrackCount(Album album)
    {
        album.NumberOfTracks++;
        _context.SaveChanges();
    }
}