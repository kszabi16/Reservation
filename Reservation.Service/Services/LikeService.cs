using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reservation.DataContext.Context;
using Reservation.DataContext.Dtos;
using Reservation.DataContext.Entities;
using Reservation.Service.Interfaces;

namespace Reservation.Service.Services
{
    public class LikeService : ILikeService
    {
        private readonly IMapper _mapper;
        private readonly ReservationDbContext _context;

        public LikeService(IMapper mapper, ReservationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<LikeDto>> GetLikesByUserIdAsync(int userId)
        {
            var likes = await _context.Likes
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
            return _mapper.Map<IEnumerable<LikeDto>>(likes);
        }

        public async Task<bool> DeleteLikeAsync(int id)
        {
            var like = await _context.Likes.FindAsync(id);
            if (like == null)
                return false;

            like.Deleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleLikeAsync(int userId, LikeTargetType targetType, int? propertyId = null, int? commentId = null)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            if (targetType == LikeTargetType.Property && propertyId.HasValue)
            {
                var property = await _context.Properties.FindAsync(propertyId.Value);
                if (property == null)
                    throw new InvalidOperationException("Property not found.");
            }
            else if (targetType == LikeTargetType.Comment && commentId.HasValue)
            {
                var comment = await _context.Comments.FindAsync(commentId.Value);
                if (comment == null)
                    throw new InvalidOperationException("Comment not found.");
            }

            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.TargetType == targetType &&
                    (targetType == LikeTargetType.Property ? l.PropertyId == propertyId : l.CommentId == commentId));

            if (existingLike != null)
            {
                existingLike.Deleted = true;
                await _context.SaveChangesAsync();
                return false;
            }
            else
            {
                var like = new Like
                {
                    UserId = userId,
                    TargetType = targetType,
                    PropertyId = propertyId,
                    CommentId = commentId
                };
                _context.Likes.Add(like);
                await _context.SaveChangesAsync();
                return true;
            }
        }
    }
}
