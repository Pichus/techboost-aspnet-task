using Microsoft.EntityFrameworkCore;
using techboost_aspnet.Data;
using techboost_aspnet.Entities;
using techboost_aspnet.Exceptions;

namespace techboost_aspnet.Repositories;

public class PlaylistRepository
{
    private readonly MusicCollectionDbContext _context;

    public PlaylistRepository(MusicCollectionDbContext context)
    {
        _context = context;
    }

    public void CreatePlaylist(string name, User user)
    {
        bool playlistExists = _context.Playlists.Any(playlist => playlist.UserId == user.Id && playlist.Name == name);

        if (playlistExists)
        {
            throw new EntityAlreadyExistsException(name);
        }

        var playlistToAdd = new Playlist()
        {
            Name = name,
            User = user,
            CreatedAt = DateTime.UtcNow
        };

        _context.Playlists.Add(playlistToAdd);
        _context.SaveChanges();
    }

    public ICollection<Playlist> GetPlaylistsByUser(User user)
    {
        var results = _context.Playlists.Where(playlist => playlist.UserId == user.Id).ToList();

        return results;
    }

    public Playlist GetPlaylistByUserAndName(User user, string name)
    {
        var result = _context.Playlists.Include(playlist => playlist.Tracks).FirstOrDefault(
            playlist => playlist.UserId == user.Id && playlist.Name == name);

        if (result is null)
        {
            throw new EntityNotFoundException(name);
        }

        return result;
    }

    public void AddTrackToPlaylist(Playlist playlist, Track track)
    {
        if (playlist.Tracks.Contains(track))
        {
            throw new EntityAlreadyExistsException(track.Title);
        }
        
        playlist.Tracks.Add(track);
    }

    public ICollection<Track> GetPlaylistTracks(User user, string name)
    {
        var resultPlaylist =
            _context.Playlists.Include(playlist => playlist.Tracks)
                .FirstOrDefault(playlist => playlist.UserId == user.Id && playlist.Name == name);

        if (resultPlaylist is null)
        {
            throw new EntityNotFoundException(name);
        }

        return resultPlaylist.Tracks.ToList();
    }
}