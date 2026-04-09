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

            if (createCommentDto.Rating > 0)
            {
                var rating = new Rating
                {
                    PropertyId = createCommentDto.PropertyId,
                    UserId = userId,
                    Score = createCommentDto.Rating,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Ratings.Add(rating);
            }

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
                .Include(c => c.User)
                .Where(c => c.PropertyId == propertyId)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
            return _mapper.Map<IEnumerable<CommentDto>>(comments);
        }

    }
}
