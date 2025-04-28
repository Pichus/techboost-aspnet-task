using System.ComponentModel.DataAnnotations;

namespace techboost_aspnet.Dto;

public class ArtistDto
{
    [MaxLength(250)] public string Name { get; set; }

    public string Country { get; set; }
    public int YearsActiveStart { get; set; }
    public int YearsActiveEnd { get; set; } // 0 if active in present
    public string Biography { get; set; }

    public string[] GenreNames { get; set; }
}