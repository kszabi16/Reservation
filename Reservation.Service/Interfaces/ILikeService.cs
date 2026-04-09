using Reservation.DataContext.Dtos;
using Reservation.DataContext.Entities;

namespace Reservation.Service.Interfaces
{
    public interface ILikeService
    {
        Task<IEnumerable<LikeDto>> GetAllLikesAsync();
        Task<LikeDto?> GetLikeByIdAsync(int id);
        Task<LikeDto> CreateLikeAsync(CreateLikeDto createLikeDto);
        Task<bool> DeleteLikeAsync(int id);
        Task<bool> IsLikedAsync(int userId, LikeTargetType targetType, int? propertyId = null, int? commentId = null);
        Task<bool> RemoveLikeAsync(int userId, LikeTargetType targetType, int? propertyId = null, int? commentId = null);
        Task<bool> ToggleLikeAsync(int userId, LikeTargetType targetType, int? propertyId = null, int? commentId = null);
    }
}
