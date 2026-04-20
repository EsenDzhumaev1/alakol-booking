using Alakol.Models.Enums;
namespace Alakol.Models;

public class Booking
{
    public int Id { get; set; }

    public int RoomId { get; set; }
    public Room Room { get; set; }

    public string GuestName { get; set; }
    public string GuestEmail { get; set; }
    public string GuestPhone { get; set; }

    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }

    public decimal TotalPrice { get; set; }

    public BookingStatus Status { get; set; } = BookingStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<BookingAddon> BookingAddons { get; set; } = new List<BookingAddon>();
}