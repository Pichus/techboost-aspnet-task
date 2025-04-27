using techboost_aspnet.Data;
using techboost_aspnet.Entities;
using techboost_aspnet.Exceptions;

namespace techboost_aspnet.Repositories;

public class UserRepository
{
    private readonly MusicCollectionDbContext _context;

    public UserRepository(MusicCollectionDbContext context)
    {
        _context = context;
    }

    private bool UserExists(string name)
    {
        return _context.Users.Any(user => user.Name == name);
    }

    void ValidateStatusString(string status)
    {
        string[] statuses = ["Bought", "Wish"];
        if (!statuses.Contains(status))
        {
            throw new ArgumentException("status should be either 'Bought' or 'Wish'");
        }
    }

    public bool AddUser(string name)
    {
        if (UserExists(name)) throw new EntityAlreadyExistsException();

        _context.Users.Add(new User
        {
            Name = name
        });
        _context.SaveChanges();

        return true;
    }

    public User GetUserByName(string name)
    {
        var result = _context.Users.FirstOrDefault(user => user.Name == name);

        if (result is null)
        {
            throw new EntityNotFoundException(name);
        }
        
        return result;
    }

    public void AddAlbumToCollection(User user, Album album, string status)
    {
        ValidateStatusString(status);

        _context.UserCollections.Add(new UserCollection
        {
            UserId = user.Id,
            AlbumId = album.Id,
            AddedAt = DateTime.Now,
            Status = status
        });
        _context.SaveChanges();
    }

    public void UpdateAlbumStatusInUserCollection(User user, Album album, string status)
    {
        ValidateStatusString(status);
        
        var collection = GetUserCollectionByAlbum(user, album);
        collection.Status = status;
        _context.SaveChanges();
    }

    public List<UserCollection> GetUserCollections(User user)
    {
        var result = _context.UserCollections.Where(
            userCollection => userCollection.UserId == user.Id).ToList();

        if (result.Count == 0)
        {
            throw new UserCollectionEmpty(user.Name);
        }
        
        return result;
    }
    
    public UserCollection GetUserCollectionByAlbum(User user, Album album)
    {
        var result = _context.UserCollections.FirstOrDefault(
            userCollection => userCollection.AlbumId == album.Id);

        if (result is null)
        {
            throw new EntityNotFoundException(user.Name);
        }
        
        return result;
    }
}