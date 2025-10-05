using Reservation.DataContext.Dtos;

namespace Reservation.Service.Interfaces
{
    public interface IFavoriteService
    {
        Task<IEnumerable<FavoriteDto>> GetAllFavoritesAsync();
        Task<FavoriteDto?> GetFavoriteByIdAsync(int id);
        Task<FavoriteDto> CreateFavoriteAsync(CreateFavoriteDto createFavoriteDto);
        Task<bool> DeleteFavoriteAsync(int id);
        Task<IEnumerable<FavoriteDto>> GetFavoritesByUserIdAsync(int userId);
        Task<IEnumerable<FavoriteDto>> GetFavoritesByPropertyIdAsync(int propertyId);
        Task<bool> IsFavoriteAsync(int userId, int propertyId);
        Task<bool> RemoveFavoriteAsync(int userId, int propertyId);
        Task<bool> ToggleFavoriteAsync(int userId, int propertyId);
    }
}
