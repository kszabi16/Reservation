using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Reservation.DataContext.Dtos;
using Reservation.DataContext.Entities;
using Reservation.Service.Interfaces;

namespace Reservation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpGet("property/{propertyId}")]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetLikesByProperty(int propertyId)
            => Ok(await _likeService.GetLikesByPropertyIdAsync(propertyId));

        [Authorize]
        [HttpPost("toggle")]
        public async Task<ActionResult> ToggleLike([FromQuery] LikeTargetType targetType, [FromQuery] int targetId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var isLiked = await _likeService.ToggleLikeAsync(userId, targetType, targetType == LikeTargetType.Property ? targetId : null, targetType == LikeTargetType.Comment ? targetId : null);
            return Ok(new { isLiked });
        }
    }
}
