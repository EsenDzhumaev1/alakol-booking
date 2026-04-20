namespace Alakol.Models;

public class BookingAddon
{
    public int BookingId { get; set; }
    public Booking Booking { get; set; }

    public int AddonId { get; set; }
    public Addon Addon { get; set; }
}