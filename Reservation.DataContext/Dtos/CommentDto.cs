

using Reservation.DataContext.Entities;

namespace Reservation.DataContext.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public int PropertyId { get; set; }
    }

    public class CreateCommentDto
    {
        public string Content { get; set; } = null!;
        public int PropertyId { get; set; }
        public int Rating { get; set; }


    }

}
