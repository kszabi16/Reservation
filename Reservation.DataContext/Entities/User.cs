namespace Reservation.DataContext.Entities
{
    public enum RoleType
    {
        Admin,
        Host,
        Guest
    }

    public class User : AbstractEntity
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public RoleType Role { get; set; }
        public bool IsTrustedHost { get; set; } = false;

        // Kapcsolatok
        public ICollection<Property> Properties { get; set; } = new List<Property>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }

}
