using Microsoft.EntityFrameworkCore;
using techboost_aspnet.Data;
using techboost_aspnet.Entities;
using techboost_aspnet.Exceptions;

namespace techboost_aspnet.Repositories;

public class UserCollectionRepository
{
    private readonly MusicCollectionDbContext _context;

    public UserCollectionRepository(MusicCollectionDbContext context)
    {
        _context = context;
    }

    public ICollection<UserCollection> GetAllCollectionEntriesByUser(User user)
    {
        var results = _context.UserCollections.Where(collection => collection.UserId == user.Id)
            .Include(collection => collection.User)
            .Include(collection => collection.Album)
            .ToList();

        return results;
    }

    public void CreateCollectionEntry(User user, Album album, string status)
    {
        ValidateStatus(status);

        bool entryExists = _context.UserCollections.Any(entry => entry.AlbumId == album.Id);

        if (entryExists)
        {
            throw new EntityAlreadyExistsException(album.Title);
        }

        var entryToAdd = new UserCollection()
        {
            User = user,
            Album = album,
            AddedAt = DateTime.UtcNow,
            Status = status
        };

        _context.UserCollections.Add(entryToAdd);
        _context.SaveChanges();
    }

    public void ChangeAlbumStatus(User user, Album album)
    {
        var entry = _context.UserCollections.FirstOrDefault(collection =>
            collection.UserId == user.Id && collection.AlbumId == album.Id);

        if (entry is null)
        {
            throw new EntityNotFoundException(album.Title);
        }

        entry.Status = entry.Status == "Bought" ? "Wish" : "Bought";

        _context.SaveChanges();
    }

    private void ValidateStatus(string status)
    {
        string[] statusTypes = ["Bought", "Wish"];

        if (!statusTypes.Contains(status))
        {
            throw new ArgumentException("status must be either Bought or Wish");
        }
    }
}