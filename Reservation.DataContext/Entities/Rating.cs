using System;

namespace Reservation.DataContext.Entities
{
    public class Rating : AbstractEntity
    {
        public int Score { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;
    }
}
