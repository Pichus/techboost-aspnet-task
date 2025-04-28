using System.ComponentModel.DataAnnotations;
using System.Text;

namespace techboost_aspnet.Entities;

public class Feedback
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int AlbumId { get; set; }

    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
    public int Rating { get; set; }

    public string? Comment { get; set; }

    public User User { get; set; }
    public Album Album { get; set; }

    public override string ToString()
    {
        const int maxCommentSnippetLength = 30;
        var commentDisplay = "null";

        if (Comment != null)
            commentDisplay = Comment.Length <= maxCommentSnippetLength
                ? $"'{Comment}'"
                : $"'{Comment.Substring(0, maxCommentSnippetLength)}...'";

        var sb = new StringBuilder();
        sb.Append("Feedback(Id=").Append(Id);
        sb.Append(", UserId=").Append(UserId);
        sb.Append(", AlbumId=").Append(AlbumId);
        sb.Append(", Rating=").Append(Rating);
        sb.Append(", Comment=").Append(commentDisplay).Append(")");
        return sb.ToString();
    }
}