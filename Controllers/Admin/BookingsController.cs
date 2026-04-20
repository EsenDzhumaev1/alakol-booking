using Alakol.Data;
using Alakol.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alakol.Controllers.Admin
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Bookings.ToListAsync();

            return View("~/Views/Admin/Bookings/Index.cshtml", bookings);
        }

        public async Task<IActionResult> Approve(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
                return NotFound();

            booking.Status = BookingStatus.Approved;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Reject(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
                return NotFound();

            booking.Status = BookingStatus.Rejected;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
                return NotFound();

            booking.Status = BookingStatus.Cancelled;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}