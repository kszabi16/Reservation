using Reservation.DataContext.Entities;

namespace Reservation.DataContext.Dtos
{
    public class BookingDto
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int GuestId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public BookingStatus Status { get; set; }
    }

    public class CreateBookingDto
    {
        public int PropertyId { get; set; }
        public int GuestId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

}
