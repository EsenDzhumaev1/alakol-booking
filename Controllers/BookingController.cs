using Alakol.DTOs;
using Alakol.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Alakol.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // GET: /Booking/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Booking/Create
        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingDto dto)
        {
            try
            {
                var bookingId = await _bookingService.CreateBookingAsync(dto);

                return RedirectToAction("Success", new { id = bookingId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }

        // GET: /Booking/Success
        public IActionResult Success(int id)
        {
            ViewBag.BookingId = id;
            return View();
        }

        // API-style endpoint (useful for testing)
        [HttpGet]
        public async Task<IActionResult> CheckAvailability(int roomId, DateTime checkIn, DateTime checkOut)
        {
            var available = await _bookingService.IsRoomAvailable(roomId, checkIn, checkOut);
            return Json(new { available });
        }
    }
}