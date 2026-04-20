using Alakol.Data;
using Alakol.DTOs;
using Alakol.Models;
using Alakol.Models.Enums;
using Alakol.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace Alakol.Services;

public class BookingService : IBookingService
{
    private readonly ApplicationDbContext _context;

    public BookingService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Check availability
    public async Task<bool> IsRoomAvailable(int roomId, DateTime checkIn, DateTime checkOut)
    {
        return !await _context.Bookings
            .AnyAsync(b =>
                b.RoomId == roomId &&
                b.Status != BookingStatus.Cancelled &&
                b.Status != BookingStatus.Rejected &&
                checkIn < b.CheckOutDate &&
                checkOut > b.CheckInDate
            );
    }

    // Calculate price
    public async Task<decimal> CalculateTotalPrice(int roomId, DateTime checkIn, DateTime checkOut, List<int> addonIds)
    {
        var room = await _context.Rooms.FindAsync(roomId);
        if (room == null)
            throw new Exception("Room not found");

        int nights = (checkOut - checkIn).Days;
        if (nights <= 0)
            throw new Exception("Invalid dates");

        decimal total = nights * room.PricePerNight;

        if (addonIds != null && addonIds.Any())
        {
            var addons = await _context.Addons
                .Where(a => addonIds.Contains(a.Id))
                .ToListAsync();

            foreach (var addon in addons)
            {
                total += addon.Price * nights;
            }
        }

        return total;
    }

    // Create booking
    public async Task<int> CreateBookingAsync(CreateBookingDto dto)
    {
        // Validate dates
        if (dto.CheckInDate >= dto.CheckOutDate)
            throw new Exception("Check-out must be after check-in");

        if (dto.CheckInDate < DateTime.UtcNow.Date)
            throw new Exception("Cannot book in the past");

        // Check availability
        var available = await IsRoomAvailable(dto.RoomId, dto.CheckInDate, dto.CheckOutDate);
        if (!available)
            throw new Exception("Room is not available");

        // Calculate price
        var totalPrice = await CalculateTotalPrice(
            dto.RoomId,
            dto.CheckInDate,
            dto.CheckOutDate,
            dto.AddonIds
        );

        var booking = new Booking
        {
            RoomId = dto.RoomId,
            GuestName = dto.GuestName,
            GuestEmail = dto.GuestEmail,
            GuestPhone = dto.GuestPhone,
            CheckInDate = dto.CheckInDate,
            CheckOutDate = dto.CheckOutDate,
            TotalPrice = totalPrice,
            Status = BookingStatus.Pending
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        // Add addons
        if (dto.AddonIds != null && dto.AddonIds.Any())
        {
            var bookingAddons = dto.AddonIds.Select(addonId => new BookingAddon
            {
                BookingId = booking.Id,
                AddonId = addonId
            });

            _context.BookingAddons.AddRange(bookingAddons);
            await _context.SaveChangesAsync();
        }

        return booking.Id;
    }
}