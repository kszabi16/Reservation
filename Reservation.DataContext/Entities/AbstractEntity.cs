namespace Reservation.DataContext.Entities
{
    public abstract class AbstractEntity
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }

    }
}
