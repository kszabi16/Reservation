using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Reservation.DataContext.Dtos;
using Reservation.Service.Interfaces;

namespace Reservation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [Authorize(Roles = "Guest,Host,Admin")]
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<FavoriteDto>>> GetFavoritesByUser(int userId)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var currentRole = User.FindFirstValue(ClaimTypes.Role);

            if (currentUserId != userId && currentRole != "Admin")
                return Forbid("You are not authorized to view another user's favorites.");

            var favorites = await _favoriteService.GetFavoritesByUserIdAsync(userId);
            return Ok(favorites);
        }

        [Authorize(Roles = "Guest,Host,Admin")]
        [HttpGet("is-favorite/{propertyId}")]
        public async Task<ActionResult<bool>> IsFavorite(int propertyId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var isFavorite = await _favoriteService.IsFavoriteAsync(userId, propertyId);
            return Ok(isFavorite);
        }

        [Authorize(Roles = "Guest,Host,Admin")]
        [HttpPost("toggle/{propertyId}")]
        public async Task<ActionResult> ToggleFavorite(int propertyId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var isFavorite = await _favoriteService.ToggleFavoriteAsync(userId, propertyId);

            return Ok(new
            {
                isFavorite,
                message = isFavorite ? "Added to favorites." : "Removed from favorites."
            });
        }
    }
}
