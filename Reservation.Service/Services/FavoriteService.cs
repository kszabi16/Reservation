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

        // 1. Kedvencek lekérdezése adott felhasználóhoz
        public async Task<IEnumerable<FavoriteDto>> GetFavoritesByUserIdAsync(int userId)
        {
            var favorites = await _context.Favorites
                .Include(f => f.Property) // <--- EZ NAGYON FONTOS: Behozza az ingatlan adatait is a frontendnek!
                .Where(f => f.UserId == userId && f.Deleted == false) // Csak a nem törölt kedvencek!
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();

            return _mapper.Map<IEnumerable<FavoriteDto>>(favorites);
        }

        // 2. Ellenőrzés, hogy kedvenc-e
        public async Task<bool> IsFavoriteAsync(int userId, int propertyId)
        {
            // Csak akkor kedvenc, ha létezik ÉS nincs törölve
            return await _context.Favorites
                .AnyAsync(f => f.UserId == userId && f.PropertyId == propertyId && f.Deleted == false);
        }

        // 3. Hozzáadás / Elvétel (Toggle)
        public async Task<bool> ToggleFavoriteAsync(int userId, int propertyId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            var property = await _context.Properties.FindAsync(propertyId);
            if (property == null)
                throw new InvalidOperationException("Property not found.");

            // Megkeressük, hogy volt-e már valaha kedvencek között ez az ingatlan
            var existingFavorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.PropertyId == propertyId);

            if (existingFavorite != null)
            {
                // Ha megvan, megfordítjuk a státuszát
                if (existingFavorite.Deleted == true)
                {
                    existingFavorite.Deleted = false; // Visszaállítjuk
                    await _context.SaveChangesAsync();
                    return true; // Újra kedvenc lett
                }
                else
                {
                    existingFavorite.Deleted = true; // Töröljük
                    await _context.SaveChangesAsync();
                    return false; // Nem kedvenc többé
                }
            }
            else
            {
                // Ha még sosem volt kedvenc, létrehozzuk
                var favorite = new Favorite
                {
                    UserId = userId,
                    PropertyId = propertyId,
                    Deleted = false,
                    CreatedAt = DateTime.UtcNow // Biztosítjuk, hogy legyen dátuma
                };

                _context.Favorites.Add(favorite);
                await _context.SaveChangesAsync();

                return true; // Kedvenc lett
            }
        }
    }
}