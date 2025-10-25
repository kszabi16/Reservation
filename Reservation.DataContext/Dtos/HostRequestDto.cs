namespace Reservation.DataContext.Dtos
{
    public class HostRequestDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public int PropertyId { get; set; }
        public string PropertyTitle { get; set; } = null!;
        public bool IsApproved { get; set; }
        public DateTime RequestedAt { get; set; }
    }

    public class CreateHostRequestDto
    {
        public int UserId { get; set; }
        public int PropertyId { get; set; }
    }
}
