using Alakol.Data;
using Alakol.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alakol.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuestHousesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GuestHousesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/guesthouses
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var guestHouses = await _context.GuestHouses.ToListAsync();
            return Ok(guestHouses);
        }

        // GET: api/guesthouses/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var guestHouse = await _context.GuestHouses
                .Include(g => g.Rooms) // optional but useful
                .FirstOrDefaultAsync(g => g.Id == id);

            if (guestHouse == null)
                return NotFound();

            return Ok(guestHouse);
        }

        // POST: api/guesthouses
        [HttpPost]
        public async Task<IActionResult> Create(GuestHouse guestHouse)
        {
            _context.GuestHouses.Add(guestHouse);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = guestHouse.Id }, guestHouse);
        }

        // PUT: api/guesthouses/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, GuestHouse updated)
        {
            var guestHouse = await _context.GuestHouses.FindAsync(id);

            if (guestHouse == null)
                return NotFound();

            guestHouse.Name = updated.Name;
            guestHouse.Address = updated.Address;
            guestHouse.Description = updated.Description;
            guestHouse.Phone = updated.Phone;
            guestHouse.Email = updated.Email;

            await _context.SaveChangesAsync();

            return Ok(guestHouse);
        }

        // DELETE: api/guesthouses/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var guestHouse = await _context.GuestHouses.FindAsync(id);

            if (guestHouse == null)
                return NotFound();

            _context.GuestHouses.Remove(guestHouse);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}