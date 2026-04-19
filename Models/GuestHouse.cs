using Alakol.Models;

public class GuestHouse
{
    public int Id { get; set; }

    public string Name { get; set; } = "Alakol";
    public string Address { get; set; } = "Stahanova 140, Karakol";

    public string? Description { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Room> Rooms { get; set; } = new List<Room>();
}