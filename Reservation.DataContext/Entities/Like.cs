namespace Reservation.DataContext.Entities
{
    public enum LikeTargetType
    {
        Property,
        Comment
    }

    public class Like : AbstractEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public LikeTargetType TargetType { get; set; }

        // Kapcsolatok
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // opcionális hivatkozások
        public int? PropertyId { get; set; }
        public Property? Property { get; set; }

        public int? CommentId { get; set; }
        public Comment? Comment { get; set; }
    }

}
