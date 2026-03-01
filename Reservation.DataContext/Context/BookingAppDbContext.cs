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

            // Decimal precision beállítása a PricePerNight mezőhöz
            modelBuilder.Entity<Property>()
                .Property(p => p.PricePerNight)
                .HasPrecision(18, 2);

            // HostRequest -> Property kapcsolat
            modelBuilder.Entity<HostRequest>()
                .HasOne(r => r.Property)
                .WithMany()
                .HasForeignKey(r => r.PropertyId)
                .OnDelete(DeleteBehavior.NoAction);

            // HostRequest -> User kapcsolat
            modelBuilder.Entity<HostRequest>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Rating>()
                .HasQueryFilter(e => !e.Deleted);



            modelBuilder.Entity<Property>().HasData(
                new Property
                {
                    Id = 1,
                    Title = "Budapest belvárosi apartman",
                    Description = "Modern apartman a Deák tér közelében, remek kilátással.",
                    Location = "Budapest",
                    PricePerNight = 25000m,
                    Capacity = 3,
                    CreatedAt = new DateTime(2024, 10, 14, 0, 0, 0, DateTimeKind.Utc), // FIX UTC DÁTUM
                    HostId = 2,
                    IsApproved = true
                },
                new Property
                {
                    Id = 2,
                    Title = "Balatoni nyaraló",
                    Description = "Kényelmes ház közvetlenül a Balaton partján.",
                    Location = "Siófok",
                    PricePerNight = 45000m,
                    Capacity = 6,
                    CreatedAt = new DateTime(2024, 10, 14, 0, 0, 0, DateTimeKind.Utc), // FIX UTC DÁTUM
                    HostId = 2,
                    IsApproved = true
                },
                new Property
                {
                    Id = 3,
                    Title = "Hegyi faház Mátrában",
                    Description = "Hangulatos faház kandallóval és panorámás terasszal.",
                    Location = "Mátraháza",
                    PricePerNight = 32000m,
                    Capacity = 5,
                    CreatedAt = new DateTime(2024, 10, 14, 0, 0, 0, DateTimeKind.Utc), // FIX UTC DÁTUM
                    HostId = 2,
                    IsApproved = true
                }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "Kovács Béla",
                    Email = "bela@example.com",
                    PasswordHash = "hashed_pw",
                    Role = RoleType.Host,
                    CreatedAt = new DateTime(2024, 10, 14, 0, 0, 0, DateTimeKind.Utc) // EZ HIÁNYZOTT!
                },
                new User
                {
                    Id = 2,
                    Username = "Tóth Anna",
                    Email = "anna@example.com",
                    PasswordHash = "hashed_pw",
                    Role = RoleType.Host,
                    CreatedAt = new DateTime(2024, 10, 14, 0, 0, 0, DateTimeKind.Utc) // EZ HIÁNYZOTT!
                }
            );


        }

    }
}
