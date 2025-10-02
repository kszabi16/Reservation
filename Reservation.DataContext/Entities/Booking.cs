namespace Reservation.DataContext.Entities
{
    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Cancelled
    }

    public class Booking : AbstractEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Kapcsolatok
        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;

        public int GuestId { get; set; }
        public User Guest { get; set; } = null!;
    }

}
