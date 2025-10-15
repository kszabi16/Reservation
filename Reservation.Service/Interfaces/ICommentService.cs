using Reservation.DataContext.Dtos;

namespace Reservation.Service.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDto>> GetAllCommentsAsync();
        Task<CommentDto?> GetCommentByIdAsync(int id);
        Task<CommentDto> CreateCommentAsync(CreateCommentDto createCommentDto,int userId);
        Task<CommentDto?> UpdateCommentAsync(int id, CreateCommentDto updateCommentDto);
        Task<bool> DeleteCommentAsync(int id);
        Task<IEnumerable<CommentDto>> GetCommentsByPropertyIdAsync(int propertyId);
        Task<IEnumerable<CommentDto>> GetCommentsByUserIdAsync(int userId);
        Task<IEnumerable<CommentDto>> GetCommentRepliesAsync(int parentCommentId);
        Task<IEnumerable<CommentDto>> GetTopLevelCommentsAsync(int propertyId);
    }
}
