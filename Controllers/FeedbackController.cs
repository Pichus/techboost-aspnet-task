using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using techboost_aspnet.Data;
using techboost_aspnet.Dto;
using techboost_aspnet.Entities;

namespace techboost_aspnet.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FeedbackController : ControllerBase
{
    private readonly MusicCollectionDbContext _context;

    public FeedbackController(MusicCollectionDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacks()
    {
        return await _context.Feedbacks.ToListAsync();
    }

    [HttpGet("album/{id}")]
    public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbackByAlbumId(int id)
    {
        var feedbacks = await _context.Feedbacks.Where(feedback => feedback.AlbumId == id).ToListAsync();

        if (feedbacks.Count == 0) return NotFound($"no feedbacks for album with id {id}");

        return feedbacks;
    }

    [HttpGet("user/{id}")]
    public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbackByUserId(int id)
    {
        var feedbacks = await _context.Feedbacks.Where(feedback => feedback.UserId == id).ToListAsync();

        if (feedbacks.Count == 0) return NotFound($"no feedbacks for user with id {id}");

        return feedbacks;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Feedback>> GetFeedback(int id)
    {
        var feedback = await _context.Feedbacks.FindAsync(id);

        if (feedback == null) return NotFound();

        return feedback;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutFeedback(int id, FeedbackDto feedbackDto)
    {
        var feedback = DtoToEntity(feedbackDto);

        _context.Entry(feedback).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!FeedbackExists(id)) return NotFound();

            throw;
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Feedback>> PostFeedback(FeedbackDto feedbackDto)
    {
        var feedback = DtoToEntity(feedbackDto);

        _context.Feedbacks.Add(feedback);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetFeedback", new { id = feedback.Id }, feedback);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFeedback(int id)
    {
        var feedback = await _context.Feedbacks.FindAsync(id);
        if (feedback == null) return NotFound();

        _context.Feedbacks.Remove(feedback);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool FeedbackExists(int id)
    {
        return _context.Feedbacks.Any(e => e.Id == id);
    }

    private Feedback DtoToEntity(FeedbackDto feedbackDto)
    {
        var feedback = new Feedback
        {
            UserId = feedbackDto.UserId,
            AlbumId = feedbackDto.AlbumId,
            Rating = feedbackDto.Rating,
            Comment = feedbackDto.Comment
        };

        return feedback;
    }
}