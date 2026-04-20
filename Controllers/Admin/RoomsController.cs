using Alakol.Data;
using Alakol.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alakol.Controllers.Admin
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public RoomsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var rooms = await _context.Rooms
                .Include(r => r.Images)
                .Include(r => r.GuestHouse)
                .ToListAsync();

            return View("~/Views/Admin/Rooms/Index.cshtml", rooms);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.GuestHouses = await _context.GuestHouses.ToListAsync();
            return View("~/Views/Admin/Rooms/Create.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Create(Room room, List<IFormFile> images)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            await SaveRoomImagesAsync(room.Id, images);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var room = await _context.Rooms
                .Include(r => r.Images)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null)
            {
                return NotFound();
            }

            ViewBag.GuestHouses = await _context.GuestHouses.ToListAsync();
            return View("~/Views/Admin/Rooms/Edit.cshtml", room);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Room room, List<IFormFile> images)
        {
            var existingRoom = await _context.Rooms
                .Include(r => r.Images)
                .FirstOrDefaultAsync(r => r.Id == room.Id);

            if (existingRoom == null)
            {
                return NotFound();
            }

            existingRoom.Name = room.Name;
            existingRoom.Description = room.Description;
            existingRoom.PricePerNight = room.PricePerNight;
            existingRoom.Capacity = room.Capacity;
            existingRoom.GuestHouseId = room.GuestHouseId;
            existingRoom.IsActive = room.IsActive;

            await _context.SaveChangesAsync();
            await SaveRoomImagesAsync(existingRoom.Id, images);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            var image = await _context.RoomImages.FindAsync(imageId);
            if (image == null)
            {
                return NotFound();
            }

            var roomId = image.RoomId;
            var fileName = Path.GetFileName(image.ImageUrl?.TrimStart('/'));
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var filePath = Path.Combine(_environment.WebRootPath, "uploads", fileName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.RoomImages.Remove(image);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Edit), new { id = roomId });
        }

        private async Task SaveRoomImagesAsync(int roomId, List<IFormFile> images)
        {
            if (images == null || images.Count == 0)
            {
                return;
            }

            var uploadPath = Path.Combine(_environment.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadPath);

            var hasMainImage = await _context.RoomImages.AnyAsync(i => i.RoomId == roomId && i.IsMain);

            foreach (var image in images.Where(i => i.Length > 0))
            {
                if (!image.ContentType.StartsWith("image/"))
                {
                    continue;
                }

                var extension = Path.GetExtension(image.FileName);
                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadPath, fileName);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                _context.RoomImages.Add(new RoomImage
                {
                    RoomId = roomId,
                    ImageUrl = $"/uploads/{fileName}",
                    IsMain = !hasMainImage
                });

                hasMainImage = true;
            }

            await _context.SaveChangesAsync();
        }
    }
}
