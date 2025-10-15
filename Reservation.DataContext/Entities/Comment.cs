namespace Reservation.DataContext.Entities
{
    public class Comment : AbstractEntity
    {
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Kapcsolatok
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;

        
    }

}
