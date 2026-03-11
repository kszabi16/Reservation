using System;

namespace Reservation.DataContext.Entities
{
    public class SystemLog
    {
        public int Id { get; set; }
        public string Level { get; set; } = "Info"; 
        public string Message { get; set; } = null!;
        public string? Details { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}