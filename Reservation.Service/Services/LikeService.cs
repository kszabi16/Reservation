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

        public async Task<IEnumerable<LikeDto>> GetAllLikesAsync()
        {
            var likes = await _context.Likes.ToListAsync();
            return _mapper.Map<IEnumerable<LikeDto>>(likes);
        }

        public async Task<LikeDto?> GetLikeByIdAsync(int id)
        {
            var like = await _context.Likes.FindAsync(id);
            return like == null ? null : _mapper.Map<LikeDto>(like);
        }

        public async Task<LikeDto> CreateLikeAsync(CreateLikeDto createLikeDto)
        {
            var user = await _context.Users.FindAsync(createLikeDto.UserId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            // Ellenőrizzük a target entitást
            if (createLikeDto.TargetType == LikeTargetType.Property)
            {
                if (!createLikeDto.PropertyId.HasValue)
                    throw new InvalidOperationException("PropertyId is required for Property likes.");
                
                var property = await _context.Properties.FindAsync(createLikeDto.PropertyId.Value);
                if (property == null)
                    throw new InvalidOperationException("Property not found.");
            }
            else if (createLikeDto.TargetType == LikeTargetType.Comment)
            {
                if (!createLikeDto.CommentId.HasValue)
                    throw new InvalidOperationException("CommentId is required for Comment likes.");
                
                var comment = await _context.Comments.FindAsync(createLikeDto.CommentId.Value);
                if (comment == null)
                    throw new InvalidOperationException("Comment not found.");
            }

            // Ellenőrizzük, hogy már nincs-e like
            if (await IsLikedAsync(createLikeDto.UserId, createLikeDto.TargetType, createLikeDto.PropertyId, createLikeDto.CommentId))
                throw new InvalidOperationException("Already liked.");

            var like = _mapper.Map<Like>(createLikeDto);
            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            return _mapper.Map<LikeDto>(like);
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

        public async Task<IEnumerable<LikeDto>> GetLikesByUserIdAsync(int userId)
        {
            var likes = await _context.Likes
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
            return _mapper.Map<IEnumerable<LikeDto>>(likes);
        }

        public async Task<IEnumerable<LikeDto>> GetLikesByPropertyIdAsync(int propertyId)
        {
            var likes = await _context.Likes
                .Where(l => l.TargetType == LikeTargetType.Property && l.PropertyId == propertyId)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
            return _mapper.Map<IEnumerable<LikeDto>>(likes);
        }

        public async Task<IEnumerable<LikeDto>> GetLikesByCommentIdAsync(int commentId)
        {
            var likes = await _context.Likes
                .Where(l => l.TargetType == LikeTargetType.Comment && l.CommentId == commentId)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
            return _mapper.Map<IEnumerable<LikeDto>>(likes);
        }

        public async Task<IEnumerable<LikeDto>> GetLikesByTargetTypeAsync(LikeTargetType targetType)
        {
            var likes = await _context.Likes
                .Where(l => l.TargetType == targetType)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
            return _mapper.Map<IEnumerable<LikeDto>>(likes);
        }

        public async Task<bool> IsLikedAsync(int userId, LikeTargetType targetType, int? propertyId = null, int? commentId = null)
        {
            var query = _context.Likes.Where(l => l.UserId == userId && l.TargetType == targetType);
            
            if (targetType == LikeTargetType.Property && propertyId.HasValue)
                query = query.Where(l => l.PropertyId == propertyId);
            else if (targetType == LikeTargetType.Comment && commentId.HasValue)
                query = query.Where(l => l.CommentId == commentId);

            return await query.AnyAsync();
        }

        public async Task<bool> RemoveLikeAsync(int userId, LikeTargetType targetType, int? propertyId = null, int? commentId = null)
        {
            var query = _context.Likes.Where(l => l.UserId == userId && l.TargetType == targetType);
            
            if (targetType == LikeTargetType.Property && propertyId.HasValue)
                query = query.Where(l => l.PropertyId == propertyId);
            else if (targetType == LikeTargetType.Comment && commentId.HasValue)
                query = query.Where(l => l.CommentId == commentId);

            var like = await query.FirstOrDefaultAsync();
            
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

            // Ellenőrizzük a target entitást
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
                // Ha már like, akkor eltávolítjuk
                existingLike.Deleted = true;
                await _context.SaveChangesAsync();
                return false; // Nem like többé
            }
            else
            {
                // Ha nem like, akkor hozzáadjuk
                var like = new Like
                {
                    UserId = userId,
                    TargetType = targetType,
                    PropertyId = propertyId,
                    CommentId = commentId
                };
                _context.Likes.Add(like);
                await _context.SaveChangesAsync();
                return true; // Like lett
            }
        }
    }
}
