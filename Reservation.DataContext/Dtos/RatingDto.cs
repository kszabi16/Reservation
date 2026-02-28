using System;

namespace Reservation.DataContext.Dtos
{
    public class RatingDto
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public int UserId { get; set; }
        public int PropertyId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateRatingDto
    {
        public int Score { get; set; }
        public int PropertyId { get; set; }
    }
}