namespace Alakol.Models;

public class Addon
{
    public int Id { get; set; }

    public string Name { get; set; } // Lunch, Dinner
    public decimal Price { get; set; } = 6;

    public ICollection<BookingAddon> BookingAddons { get; set; } = new List<BookingAddon>();
}