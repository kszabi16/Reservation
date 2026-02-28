using Reservation.DataContext.Dtos;

namespace Reservation.Service.Interfaces
{
    public interface IFavoriteService
    {
        Task<IEnumerable<FavoriteDto>> GetFavoritesByUserIdAsync(int userId);
        Task<bool> IsFavoriteAsync(int userId, int propertyId);
        Task<bool> ToggleFavoriteAsync(int userId, int propertyId);
    }
}
