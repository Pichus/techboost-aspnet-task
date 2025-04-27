using System.ComponentModel.DataAnnotations;
using System.Text;

namespace techboost_aspnet.Entities;

public class Track
{
    public int Id { get; set; }

    [MaxLength(250)] public string Title { get; set; }

    public TimeSpan Duration { get; set; }
    public int AlbumId { get; set; }
    public int ChartPosition { get; set; } // 0 if not in the chart
    public string Lyricist { get; set; }

    public Album Album { get; set; }
    public ICollection<Playlist> Playlists { get; set; }
    public ICollection<Artist> Artists { get; set; }
    
    public override string ToString()
    {
        string titlePart = Title ?? "null";
        string durationPart = Duration.ToString("c"); 

        string artistsStr = "null";
        if (Artists != null) // Check collection exists
        {
             artistsStr = "[" + string.Join(", ", Artists.Select(a => a?.ToString() ?? "null")) + "]";
        }
        
        string chartPart = ChartPosition > 0 ? ChartPosition.ToString() : "N/A";
        
        string lyricistPart = string.IsNullOrEmpty(Lyricist) ? "null" : $"'{Lyricist}'";

        var sb = new StringBuilder();
        sb.Append("Track(Id=").Append(Id);
        sb.Append(", Title='").Append(titlePart).Append("'");
        sb.Append(", Artists=").Append(artistsStr); // Add formatted artist list
        sb.Append(", Duration='").Append(durationPart).Append("'");
        sb.Append(", AlbumId=").Append(AlbumId); // Use AlbumId directly
        sb.Append(", Chart='").Append(chartPart).Append("'");
        sb.Append(", Lyricist=").Append(lyricistPart).Append(")");
        return sb.ToString();
    }
}