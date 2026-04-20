using Alakol.Data;
using Alakol.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alakol.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddonsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AddonsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/addons
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var addons = await _context.Addons.ToListAsync();
            return Ok(addons);
        }

        // GET: api/addons/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var addon = await _context.Addons.FindAsync(id);

            if (addon == null)
                return NotFound();

            return Ok(addon);
        }

        // POST: api/addons
        [HttpPost]
        public async Task<IActionResult> Create(Addon addon)
        {
            _context.Addons.Add(addon);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = addon.Id }, addon);
        }

        // PUT: api/addons/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Addon updatedAddon)
        {
            var addon = await _context.Addons.FindAsync(id);

            if (addon == null)
                return NotFound();

            addon.Name = updatedAddon.Name;
            addon.Price = updatedAddon.Price;

            await _context.SaveChangesAsync();

            return Ok(addon);
        }

        // DELETE: api/addons/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var addon = await _context.Addons.FindAsync(id);

            if (addon == null)
                return NotFound();

            _context.Addons.Remove(addon);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}