using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reservation.DataContext.Context;
using Reservation.DataContext.Dtos;
using Reservation.DataContext.Entities;
using Reservation.Service.Interfaces;

namespace Reservation.Service.Services
{
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;
        private readonly ReservationDbContext _context;

        public CommentService(IMapper mapper, ReservationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<CommentDto>> GetAllCommentsAsync()
        {
            var comments = await _context.Comments.ToListAsync();
            return _mapper.Map<IEnumerable<CommentDto>>(comments);
        }

        public async Task<CommentDto?> GetCommentByIdAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            return comment == null ? null : _mapper.Map<CommentDto>(comment);
        }

        public async Task<CommentDto> CreateCommentAsync(CreateCommentDto createCommentDto, int userId)
        {
            var property = await _context.Properties.FindAsync(createCommentDto.PropertyId);
            if (property == null)
                throw new InvalidOperationException("Property not found.");

            if (string.IsNullOrWhiteSpace(createCommentDto.Content))
                throw new InvalidOperationException("Comment content cannot be empty.");

            var comment = _mapper.Map<Comment>(createCommentDto);

            
            comment.UserId = userId;
            comment.CreatedAt = DateTime.UtcNow;

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return _mapper.Map<CommentDto>(comment);
        }


        public async Task<CommentDto?> UpdateCommentAsync(int id, CreateCommentDto updateCommentDto)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
                return null;

            if (string.IsNullOrWhiteSpace(updateCommentDto.Content))
                throw new InvalidOperationException("Comment content cannot be empty.");

            _mapper.Map(updateCommentDto, comment);
            await _context.SaveChangesAsync();

            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<bool> DeleteCommentAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
                return false;

            comment.Deleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByPropertyIdAsync(int propertyId)
        {
            var comments = await _context.Comments
                .Where(c => c.PropertyId == propertyId)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
            return _mapper.Map<IEnumerable<CommentDto>>(comments);
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByUserIdAsync(int userId)
        {
            var comments = await _context.Comments
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            return _mapper.Map<IEnumerable<CommentDto>>(comments);
        }

        public async Task<IEnumerable<CommentDto>> GetCommentRepliesAsync(int CommentId)
        {
            var replies = await _context.Comments
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
            return _mapper.Map<IEnumerable<CommentDto>>(replies);
        }

        public async Task<IEnumerable<CommentDto>> GetTopLevelCommentsAsync(int propertyId)
        {
            var topLevelComments = await _context.Comments
                .Where(c => c.PropertyId == propertyId )
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
            return _mapper.Map<IEnumerable<CommentDto>>(topLevelComments);
        }
    }
}
