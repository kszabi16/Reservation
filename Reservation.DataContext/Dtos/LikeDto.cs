using Reservation.DataContext.Entities;

namespace Reservation.DataContext.Dtos
{
    public class LikeDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public LikeTargetType TargetType { get; set; }
        public int? PropertyId { get; set; }
        public int? CommentId { get; set; }
    }

    public class CreateLikeDto
    {
        public int UserId { get; set; }
        public LikeTargetType TargetType { get; set; }
        public int? PropertyId { get; set; }
        public int? CommentId { get; set; }
    }

}
