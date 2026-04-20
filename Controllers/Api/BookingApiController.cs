using Alakol.DTOs;
using Alakol.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Alakol.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingApiController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingApiController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto dto)
        {
            try
            {
                var bookingId = await _bookingService.CreateBookingAsync(dto);
                return Ok(new { bookingId });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("check-availability")]
        public async Task<IActionResult> CheckAvailability(int roomId, DateTime checkIn, DateTime checkOut)
        {
            var available = await _bookingService.IsRoomAvailable(roomId, checkIn, checkOut);
            return Ok(new { available });
        }
    }
}