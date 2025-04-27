using techboost_aspnet.Data;
using techboost_aspnet.Entities;
using techboost_aspnet.Exceptions;

namespace techboost_aspnet.Repositories;

public class FeedbackRepository
{
    private readonly MusicCollectionDbContext _context;

    public FeedbackRepository(MusicCollectionDbContext context)
    {
        _context = context;
    }

    public void CreateFeedback(User user, Album album, int rating, string? comment = null)
    {
        bool feedbackExists = _context.Feedbacks.Any(feedback => feedback.UserId == user.Id);

        if (rating > 5 || rating < 0)
        {
            throw new ArgumentException("Rating must be in range [0, 5]");
        }
        
        if (feedbackExists)
        {
            throw new EntityAlreadyExistsException(user.Name);
        }

        var feedbackToAdd = new Feedback()
        {
            Album = album,
            User = user,
            Rating = rating
        };

        if (comment is not null)
        {
            feedbackToAdd.Comment = comment;
        }

        _context.Feedbacks.Add(feedbackToAdd);
        _context.SaveChanges();
    }

    public ICollection<Feedback> GetAllFeedbackByAlbum(Album album)
    {
        var results = _context.Feedbacks.Where(feedback => feedback.AlbumId == album.Id).ToList();
        return results;
    }

    public ICollection<Feedback> GetAllFeedbackByUser(User user)
    {
        var results = _context.Feedbacks.Where(feedback => feedback.UserId == user.Id).ToList();

        return results;
    }
}