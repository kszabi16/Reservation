using System;

namespace Reservation.DataContext.Dtos
{
    public class SystemLogDto
    {
        public int Id { get; set; }
        public string Level { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string? Details { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}