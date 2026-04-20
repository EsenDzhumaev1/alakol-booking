using Alakol.Data;
using Alakol.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alakol.Controllers.Admin
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var rooms = await _context.Rooms.ToListAsync();
            return View("~/Views/Admin/Rooms/Index.cshtml", rooms);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.GuestHouses = await _context.GuestHouses.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            ViewBag.GuestHouses = await _context.GuestHouses.ToListAsync();
            return View(room);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}