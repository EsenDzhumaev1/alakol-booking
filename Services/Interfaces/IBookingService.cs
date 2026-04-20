using Alakol.DTOs;
namespace Alakol.Services.Interfaces;

public interface IBookingService
{
    Task<bool> IsRoomAvailable(int roomId, DateTime checkIn, DateTime checkOut);

    Task<decimal> CalculateTotalPrice(int roomId, DateTime checkIn, DateTime checkOut, List<int> addonIds);

    Task<int> CreateBookingAsync(CreateBookingDto dto);
}