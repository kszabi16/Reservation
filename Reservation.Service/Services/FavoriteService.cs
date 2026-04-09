using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reservation.DataContext.Context;
using Reservation.DataContext.Dtos;
using Reservation.DataContext.Entities;
using Reservation.Service.Interfaces;

namespace Reservation.Service.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IMapper _mapper;
        private readonly ReservationDbContext _context;

        public FavoriteService(IMapper mapper, ReservationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
   public async Task<IEnumerable<FavoriteDto>> GetFavoritesByUserIdAsync(int userId)
        {
            var favorites = await _context.Favorites
                .Include(f => f.Property)
                .Where(f => f.UserId == userId && f.Deleted == false) 
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();

            return _mapper.Map<IEnumerable<FavoriteDto>>(favorites);
        }
        public async Task<bool> IsFavoriteAsync(int userId, int propertyId)
        { 
            return await _context.Favorites
                .AnyAsync(f => f.UserId == userId && f.PropertyId == propertyId && f.Deleted == false);
        }
        public async Task<bool> ToggleFavoriteAsync(int userId, int propertyId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            var property = await _context.Properties.FindAsync(propertyId);
            if (property == null)
                throw new InvalidOperationException("Property not found.");

            var existingFavorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.PropertyId == propertyId);

            if (existingFavorite != null)
            {
             
                if (existingFavorite.Deleted == true)
                {
                    existingFavorite.Deleted = false; 
                    await _context.SaveChangesAsync();
                    return true; 
                }
                else
                {
                    existingFavorite.Deleted = true;
                    await _context.SaveChangesAsync();
                    return false; 
                }
            }
            else
            {
              
                var favorite = new Favorite
                {
                    UserId = userId,
                    PropertyId = propertyId,
                    Deleted = false,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Favorites.Add(favorite);
                await _context.SaveChangesAsync();

                return true;
            }
        }
    }
}