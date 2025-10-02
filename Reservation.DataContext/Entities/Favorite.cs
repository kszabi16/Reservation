namespace Reservation.DataContext.Entities
{
    public class Favorite : AbstractEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Kapcsolatok
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;
    }

}
