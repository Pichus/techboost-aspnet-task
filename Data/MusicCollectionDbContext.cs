using Microsoft.EntityFrameworkCore;
using techboost_aspnet.Entities;

namespace techboost_aspnet.Data;

public class MusicCollectionDbContext : DbContext
{
    public MusicCollectionDbContext()
    {
    }

    public MusicCollectionDbContext(DbContextOptions<MusicCollectionDbContext> options) : base(options)
    {
    }

    public DbSet<Album> Albums { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Track> Tracks { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<UserCollection> UserCollections { get; set; }
}