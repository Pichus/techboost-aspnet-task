using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using techboost_aspnet.Data;
using techboost_aspnet.Entities;

namespace techboost_aspnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCollectionController : ControllerBase
    {
        private readonly MusicCollectionDbContext _context;

        public UserCollectionController(MusicCollectionDbContext context)
        {
            _context = context;
        }

        // GET: api/UserCollection
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserCollection>>> GetUserCollections()
        {
            return await _context.UserCollections.ToListAsync();
        }

        // GET: api/UserCollection/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserCollection>> GetUserCollection(int id)
        {
            var userCollection = await _context.UserCollections.FindAsync(id);

            if (userCollection == null)
            {
                return NotFound();
            }

            return userCollection;
        }

        // PUT: api/UserCollection/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserCollection(int id, UserCollection userCollection)
        {
            if (id != userCollection.Id)
            {
                return BadRequest();
            }

            _context.Entry(userCollection).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserCollectionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserCollection
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserCollection>> PostUserCollection(UserCollection userCollection)
        {
            _context.UserCollections.Add(userCollection);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserCollection", new { id = userCollection.Id }, userCollection);
        }

        // DELETE: api/UserCollection/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserCollection(int id)
        {
            var userCollection = await _context.UserCollections.FindAsync(id);
            if (userCollection == null)
            {
                return NotFound();
            }

            _context.UserCollections.Remove(userCollection);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserCollectionExists(int id)
        {
            return _context.UserCollections.Any(e => e.Id == id);
        }
    }
}
