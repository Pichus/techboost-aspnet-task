namespace techboost_aspnet.Entities;

public class UserCollection
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int AlbumId { get; set; }
    public DateTime AddedAt { get; set; }
    public string Status { get; set; } // Bought | Wish

    public User User { get; set; }
    public Album Album { get; set; }
    
    public override string ToString()
    {
        return $"UserCollection {{ Id = {Id}, UserId = {UserId}, AlbumId = {AlbumId}, AddedAt = {AddedAt}, Status = \"{Status}\", " +
               $"User = {User?.ToString() ?? "null"}, Album = {Album?.ToString() ?? "null"} }}";
    }
}