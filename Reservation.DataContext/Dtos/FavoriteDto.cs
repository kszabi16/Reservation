
using Reservation.DataContext.Entities;

namespace Reservation.DataContext.Dtos
{
    public class FavoriteDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PropertyId { get; set; }
    }

    public class CreateFavoriteDto
    {
        public int UserId { get; set; }
        public int PropertyId { get; set; }
    }

}
