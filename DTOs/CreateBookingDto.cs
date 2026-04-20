namespace Alakol.DTOs;

public class CreateBookingDto
{
    public int RoomId { get; set; }

    public string GuestName { get; set; }
    public string GuestEmail { get; set; }
    public string GuestPhone { get; set; }

    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }

    public List<int> AddonIds { get; set; } = new();
}