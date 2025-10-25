using Reservation.DataContext.Entities;

namespace Reservation.DataContext.Dtos
{
    public class PropertyDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Location { get; set; } = null!;
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
        public int HostId { get; set; }
    }

    public class CreatePropertyDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Location { get; set; } = null!;
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
      
    }

}
