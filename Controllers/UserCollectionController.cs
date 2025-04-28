using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using techboost_aspnet.Data;
using techboost_aspnet.Dto;
using techboost_aspnet.Entities;

namespace techboost_aspnet.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserCollectionController : ControllerBase
{
    private readonly MusicCollectionDbContext _context;

    public UserCollectionController(MusicCollectionDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserCollection>>> GetUserCollections()
    {
        return await _context.UserCollections.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserCollection>> GetUserCollection(int id)
    {
        var userCollection = await _context.UserCollections.FindAsync(id);

        if (userCollection == null) return NotFound();

        return userCollection;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutUserCollection(int id, UserCollectionDto userCollectionDto)
    {
        UserCollection userCollection;

        try
        {
            userCollection = DtoToEntity(userCollectionDto);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }

        _context.Entry(userCollection).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserCollectionExists(id)) return NotFound();

            throw;
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<UserCollection>> PostUserCollection(UserCollectionDto userCollectionDto)
    {
        UserCollection userCollection;

        try
        {
            userCollection = DtoToEntity(userCollectionDto);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        
        _context.UserCollections.Add(userCollection);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetUserCollection", new { id = userCollection.Id }, userCollection);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserCollection(int id)
    {
        var userCollection = await _context.UserCollections.FindAsync(id);
        if (userCollection == null) return NotFound();

        _context.UserCollections.Remove(userCollection);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool UserCollectionExists(int id)
    {
        return _context.UserCollections.Any(e => e.Id == id);
    }

    private UserCollection DtoToEntity(UserCollectionDto userCollectionDto)
    {
        ValidateStatus(userCollectionDto.Status);
        
        var userCollection = new UserCollection
        {
            UserId = userCollectionDto.UserId,
            AlbumId = userCollectionDto.AlbumId,
            AddedAt = DateTime.UtcNow,
            Status = userCollectionDto.Status,
        };

        return userCollection;
    }
    
    private void ValidateStatus(string status)
    {
        string[] statusTypes = ["Bought", "Wish"];

        if (!statusTypes.Contains(status)) throw new ArgumentException("status must be either Bought or Wish");
    }
}