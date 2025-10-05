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

        public async Task<IEnumerable<FavoriteDto>> GetAllFavoritesAsync()
        {
            var favorites = await _context.Favorites.ToListAsync();
            return _mapper.Map<IEnumerable<FavoriteDto>>(favorites);
        }

        public async Task<FavoriteDto?> GetFavoriteByIdAsync(int id)
        {
            var favorite = await _context.Favorites.FindAsync(id);
            return favorite == null ? null : _mapper.Map<FavoriteDto>(favorite);
        }

        public async Task<FavoriteDto> CreateFavoriteAsync(CreateFavoriteDto createFavoriteDto)
        {
            var user = await _context.Users.FindAsync(createFavoriteDto.UserId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            var property = await _context.Properties.FindAsync(createFavoriteDto.PropertyId);
            if (property == null)
                throw new InvalidOperationException("Property not found.");

            // Ellenőrizzük, hogy már nincs-e kedvenc
            if (await IsFavoriteAsync(createFavoriteDto.UserId, createFavoriteDto.PropertyId))
                throw new InvalidOperationException("Property is already in favorites.");

            var favorite = _mapper.Map<Favorite>(createFavoriteDto);
            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();

            return _mapper.Map<FavoriteDto>(favorite);
        }

        public async Task<bool> DeleteFavoriteAsync(int id)
        {
            var favorite = await _context.Favorites.FindAsync(id);
            if (favorite == null)
                return false;

            favorite.Deleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<FavoriteDto>> GetFavoritesByUserIdAsync(int userId)
        {
            var favorites = await _context.Favorites
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
            return _mapper.Map<IEnumerable<FavoriteDto>>(favorites);
        }

        public async Task<IEnumerable<FavoriteDto>> GetFavoritesByPropertyIdAsync(int propertyId)
        {
            var favorites = await _context.Favorites
                .Where(f => f.PropertyId == propertyId)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
            return _mapper.Map<IEnumerable<FavoriteDto>>(favorites);
        }

        public async Task<bool> IsFavoriteAsync(int userId, int propertyId)
        {
            return await _context.Favorites.AnyAsync(f => f.UserId == userId && f.PropertyId == propertyId);
        }

        public async Task<bool> RemoveFavoriteAsync(int userId, int propertyId)
        {
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.PropertyId == propertyId);
            
            if (favorite == null)
                return false;

            favorite.Deleted = true;
            await _context.SaveChangesAsync();
            return true;
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
                // Ha már kedvenc, akkor eltávolítjuk
                existingFavorite.Deleted = true;
                await _context.SaveChangesAsync();
                return false; // Nem kedvenc többé
            }
            else
            {
                // Ha nem kedvenc, akkor hozzáadjuk
                var favorite = new Favorite
                {
                    UserId = userId,
                    PropertyId = propertyId
                };
                _context.Favorites.Add(favorite);
                await _context.SaveChangesAsync();
                return true; // Kedvenc lett
            }
        }
    }
}
