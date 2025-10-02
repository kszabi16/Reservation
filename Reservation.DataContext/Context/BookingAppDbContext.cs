using Microsoft.EntityFrameworkCore;
using Reservation.DataContext.Entities;


namespace Reservation.DataContext.Context
{
    public class ReservationDbContext : DbContext
    {
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        public ReservationDbContext(DbContextOptions<ReservationDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Globális soft delete filter
            modelBuilder.Entity<User>().HasQueryFilter(e => !e.Deleted);
            modelBuilder.Entity<Property>().HasQueryFilter(e => !e.Deleted);
            modelBuilder.Entity<Booking>().HasQueryFilter(e => !e.Deleted);
            modelBuilder.Entity<Comment>().HasQueryFilter(e => !e.Deleted);
            modelBuilder.Entity<Favorite>().HasQueryFilter(e => !e.Deleted);
            modelBuilder.Entity<Like>().HasQueryFilter(e => !e.Deleted);

            // Foreign Key konfiguráció cascade delete konfliktusok elkerülésére
            
            // Booking -> Guest kapcsolat (NoAction a cascade konfliktus elkerülésére)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Guest)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.GuestId)
                .OnDelete(DeleteBehavior.NoAction);

            // Booking -> Property kapcsolat (Cascade megtartva)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Property)
                .WithMany(p => p.Bookings)
                .HasForeignKey(b => b.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Property -> Host kapcsolat (NoAction a cascade konfliktus elkerülésére)
            modelBuilder.Entity<Property>()
                .HasOne(p => p.Host)
                .WithMany(u => u.Properties)
                .HasForeignKey(p => p.HostId)
                .OnDelete(DeleteBehavior.NoAction);

            // Comment -> ParentComment kapcsolat (Restrict)
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Decimal precision beállítása a PricePerNight mezőhöz
            modelBuilder.Entity<Property>()
                .Property(p => p.PricePerNight)
                .HasPrecision(18, 2);
        }
    }
}
