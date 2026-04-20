using Alakol.Data;
using Alakol.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alakol.Controllers.Admin
{
    public class AdminServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var services = await _context.Addons.ToListAsync();
            return View("~/Views/Admin/Services/Index.cshtml", services);
        }

        public IActionResult Create()
        {
            return View("~/Views/Admin/Services/Create.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Create(Addon service)
        {
            _context.Addons.Add(service);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var service = await _context.Addons.FindAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/Services/Edit.cshtml", service);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Addon service)
        {
            var existingService = await _context.Addons.FindAsync(service.Id);

            if (existingService == null)
            {
                return NotFound();
            }

            existingService.Name = service.Name;
            existingService.Price = service.Price;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var service = await _context.Addons.FindAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            _context.Addons.Remove(service);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
