namespace Reservation.DataContext.Entities
{
    public class PropertyImage : AbstractEntity
    {
        
        public string ImageUrl { get; set; } = null!;

        public int PropertyId { get; set; }

      
        public Property Property { get; set; } = null!;
    }
}