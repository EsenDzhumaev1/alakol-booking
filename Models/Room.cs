using Alakol.Models;

public class Room
{
    public int Id { get; set; }

    public int GuestHouseId { get; set; }
    public GuestHouse GuestHouse { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }

    public decimal PricePerNight { get; set; } = 65;

    public int Capacity { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<RoomImage> Images { get; set; } = new List<RoomImage>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}