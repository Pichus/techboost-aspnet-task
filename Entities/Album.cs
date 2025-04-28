using System.ComponentModel.DataAnnotations;
using System.Text;

namespace techboost_aspnet.Entities;

public class Album
{
    public int Id { get; set; }

    [MaxLength(250)] public string Title { get; set; }

    public int ReleaseYear { get; set; }
    public int NumberOfTracks { get; set; }

    [MaxLength(100)] public string Label { get; set; }

    public string Type { get; set; } // CD | Vinyl | Digital

    public ICollection<Genre> Genres { get; set; }
    public ICollection<Artist> Artists { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("Album(Id=").Append(Id);
        sb.Append(", Title='").Append(Title ?? "null").Append("'");
        sb.Append(", Year=").Append(ReleaseYear);
        sb.Append(", Tracks=").Append(NumberOfTracks);
        sb.Append(", Label='").Append(Label ?? "null").Append("'");
        sb.Append(", Type='").Append(Type ?? "null").Append("')");
        return sb.ToString();
    }
}