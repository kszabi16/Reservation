using Reservation.DataContext.Dtos;

namespace Reservation.Service.Interfaces
{
    public interface ICommentService
    {
        Task<CommentDto?> GetCommentByIdAsync(int id);
        Task<CommentDto> CreateCommentAsync(CreateCommentDto createCommentDto,int userId);
        Task<bool> DeleteCommentAsync(int id);
        Task<IEnumerable<CommentDto>> GetCommentsByPropertyIdAsync(int propertyId);
   
    }
}
