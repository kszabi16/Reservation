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

       [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FavoriteDto>>> GetAllFavorites()
        {
            var favorites = await _favoriteService.GetAllFavoritesAsync();
            return Ok(favorites);
        }

        
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<FavoriteDto>> GetFavorite(int id)
        {
            var favorite = await _favoriteService.GetFavoriteByIdAsync(id);
            return favorite == null ? NotFound($"Favorite with ID {id} not found.") : Ok(favorite);
        }

        
        [Authorize(Roles = "Guest,Host,Admin")]
        [HttpPost]
        public async Task<ActionResult<FavoriteDto>> CreateFavorite([FromBody] CreateFavoriteDto createFavoriteDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            createFavoriteDto.UserId = userId;

            var favorite = await _favoriteService.CreateFavoriteAsync(createFavoriteDto);
            return CreatedAtAction(nameof(GetFavorite), new { id = favorite.Id }, favorite);
        }

       
        [Authorize(Roles = "Guest,Host,Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFavorite(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _favoriteService.DeleteFavoriteAsync(id);
            return result ? NoContent() : Forbid("You are not authorized to delete this favorite.");
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

     
        [AllowAnonymous]
        [HttpGet("property/{propertyId}")]
        public async Task<ActionResult<IEnumerable<FavoriteDto>>> GetFavoritesByProperty(int propertyId)
        {
            var favorites = await _favoriteService.GetFavoritesByPropertyIdAsync(propertyId);
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
        [HttpDelete("remove/{propertyId}")]
        public async Task<ActionResult> RemoveFavorite(int propertyId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _favoriteService.RemoveFavoriteAsync(userId, propertyId);
            return result ? Ok(new { message = "Favorite removed successfully." })
                          : NotFound("Favorite not found.");
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
