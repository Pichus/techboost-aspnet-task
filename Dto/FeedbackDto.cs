using System.ComponentModel.DataAnnotations;

namespace techboost_aspnet.Dto;

public class FeedbackDto
{
    public int UserId { get; set; }
    public int AlbumId { get; set; }

    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
    public int Rating { get; set; }

    public string? Comment { get; set; }
}