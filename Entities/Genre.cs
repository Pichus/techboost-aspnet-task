using System.ComponentModel.DataAnnotations;

namespace techboost_aspnet.Entities;

public class Genre
{
    public int Id { get; set; }

    [MaxLength(100)] public required string Name { get; set; }

    public ICollection<Artist> Artists { get; set; }
    public ICollection<Album> Albums { get; set; }

    public override string ToString()
    {
        return $"[{Id}] {Name}";
    }
}