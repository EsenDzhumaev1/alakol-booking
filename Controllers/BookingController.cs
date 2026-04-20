using Alakol.Data;
using Alakol.DTOs;
using Alakol.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alakol.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBookingService _bookingService;

        public BookingController(ApplicationDbContext context, IBookingService bookingService)
        {
            _context = context;
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await LoadRoomsAsync();
            return View("~/Views/Booking/Index.cshtml", new CreateBookingDto());
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateBookingDto dto, DateTime checkIn, DateTime checkOut)
        {
            dto.CheckInDate = checkIn;
            dto.CheckOutDate = checkOut;

            var minDate = DateTime.Now.Date.AddDays(3);

            if (dto.RoomId <= 0)
            {
                ModelState.AddModelError("", "Please select a room.");
            }

            if (string.IsNullOrWhiteSpace(dto.GuestName))
            {
                ModelState.AddModelError("", "Guest name is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.GuestEmail))
            {
                ModelState.AddModelError("", "Email is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.GuestPhone))
            {
                ModelState.AddModelError("", "Phone is required.");
            }

            if (dto.CheckInDate.Date < minDate)
            {
                ModelState.AddModelError("", "Check-in must be at least 3 days from today.");
            }

            if (dto.CheckOutDate <= dto.CheckInDate)
            {
                ModelState.AddModelError("", "Check-out must be after check-in.");
            }

            if (!ModelState.IsValid)
            {
                await LoadRoomsAsync();
                return View("~/Views/Booking/Index.cshtml", dto);
            }

            try
            {
                await _bookingService.CreateBookingAsync(dto);
                return RedirectToAction(nameof(Success));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                await LoadRoomsAsync();
                return View("~/Views/Booking/Index.cshtml", dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingDto dto)
        {
            return await Index(dto, dto.CheckInDate, dto.CheckOutDate);
        }

        public IActionResult Success()
        {
            return View("~/Views/Booking/Success.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> CheckAvailability(int roomId, DateTime checkIn, DateTime checkOut)
        {
            var available = await _bookingService.IsRoomAvailable(roomId, checkIn, checkOut);
            return Json(new { available });
        }

        private async Task LoadRoomsAsync()
        {
            ViewBag.Rooms = await _context.Rooms
                .Where(r => r.IsActive)
                .ToListAsync();
        }
    }
}
