using Alakol.Data;
using Alakol.DTOs;
using Alakol.Models.Enums;
using Alakol.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alakol.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IBookingService _bookingService;

        public BookingApiController(ApplicationDbContext context, IBookingService bookingService)
        {
            _context = context;
            _bookingService = bookingService;
        }

        // GET: api/bookings
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Room)
                .Select(b => new BookingResponseDto
                {
                    Id = b.Id,
                    GuestName = b.GuestName,
                    CheckInDate = b.CheckInDate,
                    CheckOutDate = b.CheckOutDate,
                    TotalPrice = b.TotalPrice,
                    Status = b.Status.ToString(),
                    RoomName = b.Room.Name
                })
                .ToListAsync();

            return Ok(bookings);
        }

        // GET: api/bookings/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Room)
                .Where(b => b.Id == id)
                .Select(b => new BookingResponseDto
                {
                    Id = b.Id,
                    GuestName = b.GuestName,
                    CheckInDate = b.CheckInDate,
                    CheckOutDate = b.CheckOutDate,
                    TotalPrice = b.TotalPrice,
                    Status = b.Status.ToString(),
                    RoomName = b.Room.Name
                })
                .FirstOrDefaultAsync();

            if (booking == null)
                return NotFound();

            return Ok(booking);
        }

        // POST: api/bookings
        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingDto dto)
        {
            try
            {
                var id = await _bookingService.CreateBookingAsync(dto);
                return Ok(new { bookingId = id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/bookings/{id}/approve
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
                return NotFound();

            booking.Status = BookingStatus.Approved;
            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/bookings/{id}/reject
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
                return NotFound();

            booking.Status = BookingStatus.Rejected;
            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/bookings/{id}/cancel
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
                return NotFound();

            // optional rule: 24h before check-in
            if ((booking.CheckInDate - DateTime.UtcNow).TotalHours < 24)
                return BadRequest("Cannot cancel less than 24 hours before check-in");

            booking.Status = BookingStatus.Cancelled;
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/bookings/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
                return NotFound();

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}