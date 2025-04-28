using System.ComponentModel.DataAnnotations;

namespace techboost_aspnet.Dto;

public class AlbumDto
{
    [MaxLength(250)] public string Title { get; set; }

    public int ReleaseYear { get; set; }
    public int NumberOfTracks { get; set; }

    [MaxLength(100)] public string Label { get; set; }

    public string Type { get; set; } // CD | Vinyl | Digital

    public string[] GenreNames { get; set; }
    public string[] ArtistNames { get; set; }
}