namespace Reservation.DataContext.Entities
{
    public class Property : AbstractEntity
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Location { get; set; } = null!;
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Jóváhagyás státusza
        public bool IsApproved { get; set; } = false;

        // Kapcsolatok
        public int HostId { get; set; }
        public User Host { get; set; } = null!;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }

}
