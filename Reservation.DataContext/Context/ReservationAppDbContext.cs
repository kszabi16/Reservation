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
        public DbSet<HostRequest> HostRequests { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<PropertyAmenity> PropertyAmenities { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }


        public ReservationDbContext(DbContextOptions<ReservationDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasQueryFilter(e => !e.Deleted);
            modelBuilder.Entity<Property>().HasQueryFilter(e => !e.Deleted);
            modelBuilder.Entity<Booking>().HasQueryFilter(e => !e.Deleted);
            modelBuilder.Entity<Comment>().HasQueryFilter(e => !e.Deleted);
            modelBuilder.Entity<Favorite>().HasQueryFilter(e => !e.Deleted);
            modelBuilder.Entity<Like>().HasQueryFilter(e => !e.Deleted);
            modelBuilder.Entity<PropertyImage>().HasQueryFilter(e => !e.Deleted);

        modelBuilder.Entity<Booking>()
                .HasOne(b => b.Guest)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.GuestId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Property)
                .WithMany(p => p.Bookings)
                .HasForeignKey(b => b.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Property>()
                .HasOne(p => p.Host)
                .WithMany(u => u.Properties)
                .HasForeignKey(p => p.HostId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Property>()
                .Property(p => p.PricePerNight)
                .HasPrecision(18, 2);

            modelBuilder.Entity<HostRequest>()
                .HasOne(r => r.Property)
                .WithMany()
                .HasForeignKey(r => r.PropertyId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HostRequest>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Rating>()
                .HasQueryFilter(e => !e.Deleted);

            modelBuilder.Entity<PropertyAmenity>()
            .HasKey(pa => new { pa.PropertyId, pa.AmenityId });

            modelBuilder.Entity<PropertyAmenity>()
                .HasOne(pa => pa.Property)
                .WithMany(p => p.PropertyAmenities)
                .HasForeignKey(pa => pa.PropertyId);

            modelBuilder.Entity<PropertyAmenity>()
                .HasOne(pa => pa.Amenity)
                .WithMany(a => a.PropertyAmenities)
                .HasForeignKey(pa => pa.AmenityId);

            modelBuilder.Entity<PropertyAmenity>()
                .HasQueryFilter(pa => !pa.Property.Deleted);
        }

    }
}
