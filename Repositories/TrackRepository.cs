using techboost_aspnet.Data;
using techboost_aspnet.Entities;
using techboost_aspnet.Exceptions;

namespace techboost_aspnet.Repositories;

public class TrackRepository
{
    private MusicCollectionDbContext _context;

    public TrackRepository(MusicCollectionDbContext context)
    {
        _context = context;
    }

    public void CreateTrack(string title, TimeSpan duration, int chartPosition, string lyricist, Album album,
        ICollection<Artist> artists)
    {
        bool trackExists = _context.Tracks.Any(track => track.Title == title);

        if (trackExists)
        {
            throw new EntityAlreadyExistsException(title);
        }

        var trackToAdd = new Track
        {
            Title = title,
            Duration = duration,
            ChartPosition = chartPosition,
            Lyricist = lyricist,
            Album = album,
            Artists = artists
        };

        _context.Tracks.Add(trackToAdd);
        _context.SaveChanges();
    }

    public ICollection<Track> GetAllTracks()
    {
        return _context.Tracks.ToList();
    }

    public Track GetTrackByTitle(string title)
    {
        var result = _context.Tracks.FirstOrDefault(track => track.Title == title);

        if (result is null)
        {
            throw new EntityNotFoundException(title);
        }

        return result;
    }
}