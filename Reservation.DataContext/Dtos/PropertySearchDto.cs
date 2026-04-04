namespace Reservation.DataContext.Dtos
{
    public class PropertySearchDto
    {
        public string PropertyId { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public decimal PricePerNight { get; set; }
        public string AmenitiesList { get; set; }
        public string MainImageUrl { get; set; }
        public double? RelevancyScore { get; set; }
    }
}