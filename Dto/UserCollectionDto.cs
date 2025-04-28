namespace techboost_aspnet.Dto;

public class UserCollectionDto
{
    public int UserId { get; set; }
    public int AlbumId { get; set; }
    public string Status { get; set; } // Bought | Wish
}