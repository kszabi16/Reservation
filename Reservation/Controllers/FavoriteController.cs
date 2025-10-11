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

        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<FavoriteDto>>> GetMyFavorites()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Ok(await _favoriteService.GetFavoritesByUserIdAsync(userId));
        }

        [HttpPost("{propertyId}")]
        public async Task<ActionResult> ToggleFavorite(int propertyId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var isFav = await _favoriteService.ToggleFavoriteAsync(userId, propertyId);
            return Ok(new { isFav });
        }
    }
}
