using Microsoft.AspNetCore.Mvc;
using Reservation.DataContext.Dtos;
using Reservation.Service.Interfaces;

namespace Reservation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FavoriteDto>>> GetAllFavorites()
        {
            try
            {
                var favorites = await _favoriteService.GetAllFavoritesAsync();
                return Ok(favorites);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FavoriteDto>> GetFavorite(int id)
        {
            try
            {
                var favorite = await _favoriteService.GetFavoriteByIdAsync(id);
                if (favorite == null)
                    return NotFound($"Favorite with ID {id} not found.");

                return Ok(favorite);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<FavoriteDto>> CreateFavorite([FromBody] CreateFavoriteDto createFavoriteDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var favorite = await _favoriteService.CreateFavoriteAsync(createFavoriteDto);
                return CreatedAtAction(nameof(GetFavorite), new { id = favorite.Id }, favorite);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFavorite(int id)
        {
            try
            {
                var result = await _favoriteService.DeleteFavoriteAsync(id);
                if (!result)
                    return NotFound($"Favorite with ID {id} not found.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<FavoriteDto>>> GetFavoritesByUser(int userId)
        {
            try
            {
                var favorites = await _favoriteService.GetFavoritesByUserIdAsync(userId);
                return Ok(favorites);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("property/{propertyId}")]
        public async Task<ActionResult<IEnumerable<FavoriteDto>>> GetFavoritesByProperty(int propertyId)
        {
            try
            {
                var favorites = await _favoriteService.GetFavoritesByPropertyIdAsync(propertyId);
                return Ok(favorites);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("is-favorite")]
        public async Task<ActionResult<bool>> IsFavorite([FromQuery] int userId, [FromQuery] int propertyId)
        {
            try
            {
                var isFavorite = await _favoriteService.IsFavoriteAsync(userId, propertyId);
                return Ok(isFavorite);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("remove")]
        public async Task<ActionResult> RemoveFavorite([FromQuery] int userId, [FromQuery] int propertyId)
        {
            try
            {
                var result = await _favoriteService.RemoveFavoriteAsync(userId, propertyId);
                if (!result)
                    return NotFound("Favorite not found.");

                return Ok(new { message = "Favorite removed successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("toggle")]
        public async Task<ActionResult<bool>> ToggleFavorite([FromQuery] int userId, [FromQuery] int propertyId)
        {
            try
            {
                var isFavorite = await _favoriteService.ToggleFavoriteAsync(userId, propertyId);
                return Ok(new { isFavorite, message = isFavorite ? "Added to favorites." : "Removed from favorites." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
