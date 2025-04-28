using System.ComponentModel.DataAnnotations;

namespace techboost_aspnet.Dto;

public class TrackDto
{
    [MaxLength(250)] public string Title { get; set; }
    public string Duration { get; set; }
    public int AlbumId { get; set; }
    public int[] ArtistIds { get; set; }
    public int ChartPosition { get; set; } // 0 if not in the chart
    public string Lyricist { get; set; }
}