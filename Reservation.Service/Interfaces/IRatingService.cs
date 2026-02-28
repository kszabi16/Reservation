using Reservation.DataContext.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reservation.Service.Interfaces
{
    public interface IRatingService
    {
        Task<RatingDto> AddOrUpdateRatingAsync(int userId, CreateRatingDto dto);
        Task<double> GetAverageRatingForPropertyAsync(int propertyId);
        Task<IEnumerable<RatingDto>> GetRatingsForPropertyAsync(int propertyId);
    }
}