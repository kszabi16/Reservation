using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reservation.DataContext.Dtos;
using Reservation.DataContext.Entities;
using Reservation.Service.Interfaces;

namespace Reservation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [Authorize(Roles = "Guest,Host,Admin")]
        [HttpPost("toggle")]
        public async Task<ActionResult<bool>> ToggleLike(
            [FromQuery] int userId,
            [FromQuery] LikeTargetType targetType,
            [FromQuery] int? propertyId = null,
            [FromQuery] int? commentId = null)
        {
            var isLiked = await _likeService.ToggleLikeAsync(userId, targetType, propertyId, commentId);
            return Ok(new { isLiked, message = isLiked ? "Liked." : "Unliked." });
        }
        [Authorize(Roles = "Guest,Host,Admin")]
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetLikesByUser(int userId)
        {
            var likes = await _likeService.GetLikesByUserIdAsync(userId);
            return Ok(likes);
        }
    }
}
