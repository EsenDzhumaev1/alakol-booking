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
        public async Task<IActionResult> Index(CreateBookingDto dto)
        {
            try
            {
                await _bookingService.CreateBookingAsync(dto);

                ModelState.Clear();
                ViewBag.SuccessMessage = "Your booking request has been sent. Waiting for confirmation.";
                await LoadRoomsAsync();
                return View("~/Views/Booking/Index.cshtml", new CreateBookingDto());
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
            return await Index(dto);
        }

        public IActionResult Success(int id)
        {
            ViewBag.BookingId = id;
            return View();
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
