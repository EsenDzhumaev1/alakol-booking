using Alakol.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Alakol.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<GuestHouse> GuestHouses { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Addon> Addons { get; set; }
        public DbSet<BookingAddon> BookingAddons { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // BookingAddon (many-to-many)
            builder.Entity<BookingAddon>()
                .HasKey(ba => new { ba.BookingId, ba.AddonId });

            builder.Entity<BookingAddon>()
                .HasOne(ba => ba.Booking)
                .WithMany(b => b.BookingAddons)
                .HasForeignKey(ba => ba.BookingId);

            builder.Entity<BookingAddon>()
                .HasOne(ba => ba.Addon)
                .WithMany(a => a.BookingAddons)
                .HasForeignKey(ba => ba.AddonId);

            // Decimal precision (IMPORTANT for money)
            builder.Entity<Room>()
                .Property(r => r.PricePerNight)
                .HasColumnType("decimal(10,2)");

            builder.Entity<Addon>()
                .Property(a => a.Price)
                .HasColumnType("decimal(10,2)");

            builder.Entity<Booking>()
                .Property(b => b.TotalPrice)
                .HasColumnType("decimal(10,2)");
        }
    }
}