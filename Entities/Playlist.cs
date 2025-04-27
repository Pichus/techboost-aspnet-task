using System.Text;

namespace techboost_aspnet.Entities;

public class Playlist
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }

    public User User { get; set; }
    public ICollection<Track> Tracks { get; set; }
    
    public override string ToString()
    {
        string datePart = CreatedAt.ToString("o"); 
        string namePart = Name ?? "null";

        var sb = new StringBuilder();
        sb.Append("Playlist(Id=").Append(Id);
        sb.Append(", Name='").Append(namePart).Append("'");
        sb.Append(", UserId=").Append(UserId);
        sb.Append(", CreatedAt='").Append(datePart).Append("')");
        return sb.ToString();
    }
}