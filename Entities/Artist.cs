using System.ComponentModel.DataAnnotations;
using System.Text;

namespace techboost_aspnet.Entities;

public class Artist
{
    public int Id { get; set; }

    [MaxLength(250)] public string Name { get; set; }

    public string Country { get; set; }
    public int YearsActiveStart { get; set; }
    public int YearsActiveEnd { get; set; } // 0 if active in present
    public string Biography { get; set; }

    public ICollection<Genre> Genres { get; set; }
    public ICollection<Album> Albums { get; set; }
    public ICollection<Track> Tracks { get; set; }
    
    public override string ToString()
    {
        string activeYearsStr = "N/A";
        if (YearsActiveStart > 0)
        {
            if (YearsActiveEnd > 0)
            {
                activeYearsStr = (YearsActiveStart == YearsActiveEnd)
                    ? $"{YearsActiveStart}"
                    : $"{YearsActiveStart}-{YearsActiveEnd}";
            }
            else
            {
                activeYearsStr = $"{YearsActiveStart}-present";
            }
        }
        
        var sb = new StringBuilder();
        sb.Append("Artist(Id=").Append(Id);
        sb.Append(", Name='").Append(Name ?? "null").Append("'");
        sb.Append(", Country='").Append(Country ?? "null").Append("'");
        sb.Append(", Active='").Append(activeYearsStr).Append("')");
        return sb.ToString(); 
    }
}