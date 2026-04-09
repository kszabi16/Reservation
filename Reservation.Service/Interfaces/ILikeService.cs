using Reservation.DataContext.Dtos;
using Reservation.DataContext.Entities;

namespace Reservation.Service.Interfaces
{
    public interface ILikeService
    {
        Task<IEnumerable<LikeDto>> GetLikesByUserIdAsync(int userId);
        Task<bool> ToggleLikeAsync(int userId, LikeTargetType targetType, int? propertyId = null, int? commentId = null);
    }
}
