namespace Alakol.DTOs;

public class BookingResponseDto
{
    public int Id { get; set; }

    public string GuestName { get; set; }

    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }

    public decimal TotalPrice { get; set; }

    public string Status { get; set; }

    public string RoomName { get; set; }
}