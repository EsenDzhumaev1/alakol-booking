using Alakol.Data;
using Alakol.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class RoomsApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RoomsApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/rooms
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var rooms = await _context.Rooms.ToListAsync();
        return Ok(rooms);
    }

    // GET: api/rooms/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var room = await _context.Rooms.FindAsync(id);

        if (room == null)
            return NotFound();

        return Ok(room);
    }

    // POST: api/rooms
    [HttpPost]
    public async Task<IActionResult> Create(Room room)
    {
        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        return Ok(room);
    }

    // PUT: api/rooms/1
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Room updatedRoom)
    {
        var room = await _context.Rooms.FindAsync(id);

        if (room == null)
            return NotFound();

        room.Name = updatedRoom.Name;
        room.Description = updatedRoom.Description;
        room.PricePerNight = updatedRoom.PricePerNight;
        room.Capacity = updatedRoom.Capacity;
        room.IsActive = updatedRoom.IsActive;

        await _context.SaveChangesAsync();

        return Ok(room);
    }

    // DELETE: api/rooms/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var room = await _context.Rooms.FindAsync(id);

        if (room == null)
            return NotFound();

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();

        return Ok();
    }
}