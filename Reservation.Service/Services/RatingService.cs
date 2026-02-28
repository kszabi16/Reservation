using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reservation.DataContext.Context;
using Reservation.DataContext.Dtos;
using Reservation.DataContext.Entities;
using Reservation.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reservation.Service.Services
{
	public class RatingService : IRatingService
	{
		private readonly ReservationDbContext _context;
		private readonly IMapper _mapper;

		public RatingService(ReservationDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<RatingDto> AddOrUpdateRatingAsync(int userId, CreateRatingDto dto)
		{
			if (dto.Score < 1 || dto.Score > 5)
				throw new InvalidOperationException("A pontszámnak 1 és 5 között kell lennie.");

			var propertyExists = await _context.Properties.AnyAsync(p => p.Id == dto.PropertyId);
			if (!propertyExists) throw new InvalidOperationException("Property not found.");

			// Ellenõrizzük, hogy ez a user értékelt-e már
			var existingRating = await _context.Ratings
				.FirstOrDefaultAsync(r => r.UserId == userId && r.PropertyId == dto.PropertyId);

			if (existingRating != null)
			{
				// Ha már értékelt, csak frissítjük a pontszámot
				existingRating.Score = dto.Score;
				await _context.SaveChangesAsync();
				return _mapper.Map<RatingDto>(existingRating);
			}

			// Új értékelés létrehozása
			var newRating = _mapper.Map<Rating>(dto);
			newRating.UserId = userId;
			_context.Ratings.Add(newRating);
			await _context.SaveChangesAsync();

			return _mapper.Map<RatingDto>(newRating);
		}

		public async Task<double> GetAverageRatingForPropertyAsync(int propertyId)
		{
			var ratings = await _context.Ratings.Where(r => r.PropertyId == propertyId).ToListAsync();
			if (!ratings.Any()) return 0;

			return Math.Round(ratings.Average(r => r.Score), 1); // 1 tizedesjegyre kerekítve (pl. 4.8)
		}

		public async Task<IEnumerable<RatingDto>> GetRatingsForPropertyAsync(int propertyId)
		{
			var ratings = await _context.Ratings.Where(r => r.PropertyId == propertyId).ToListAsync();
			return _mapper.Map<IEnumerable<RatingDto>>(ratings);
		}
	}
}