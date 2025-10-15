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

       
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetAllLikes()
        {
            var likes = await _likeService.GetAllLikesAsync();
            return Ok(likes);
        }

       
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<LikeDto>> GetLike(int id)
        {
            var like = await _likeService.GetLikeByIdAsync(id);
            if (like == null)
                return NotFound($"Like with ID {id} not found.");
            return Ok(like);
        }

       
        [Authorize(Roles = "Guest,Host,Admin")]
        [HttpPost]
        public async Task<ActionResult<LikeDto>> CreateLike([FromBody] CreateLikeDto createLikeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var like = await _likeService.CreateLikeAsync(createLikeDto);
            return CreatedAtAction(nameof(GetLike), new { id = like.Id }, like);
        }

       
        [Authorize(Roles = "Guest,Host,Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLike(int id)
        {
            var result = await _likeService.DeleteLikeAsync(id);
            if (!result)
                return NotFound($"Like with ID {id} not found.");

            return NoContent();
        }

       
        [Authorize(Roles = "Guest,Host,Admin")]
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetLikesByUser(int userId)
        {
            var likes = await _likeService.GetLikesByUserIdAsync(userId);
            return Ok(likes);
        }

      
        [AllowAnonymous]
        [HttpGet("property/{propertyId}")]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetLikesByProperty(int propertyId)
        {
            var likes = await _likeService.GetLikesByPropertyIdAsync(propertyId);
            return Ok(likes);
        }

       
        [AllowAnonymous]
        [HttpGet("comment/{commentId}")]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetLikesByComment(int commentId)
        {
            var likes = await _likeService.GetLikesByCommentIdAsync(commentId);
            return Ok(likes);
        }

      
        [Authorize(Roles = "Admin")]
        [HttpGet("target-type/{targetType}")]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetLikesByTargetType(LikeTargetType targetType)
        {
            var likes = await _likeService.GetLikesByTargetTypeAsync(targetType);
            return Ok(likes);
        }

       
        [Authorize(Roles = "Guest,Host,Admin")]
        [HttpGet("is-liked")]
        public async Task<ActionResult<bool>> IsLiked(
            [FromQuery] int userId,
            [FromQuery] LikeTargetType targetType,
            [FromQuery] int? propertyId = null,
            [FromQuery] int? commentId = null)
        {
            var isLiked = await _likeService.IsLikedAsync(userId, targetType, propertyId, commentId);
            return Ok(isLiked);
        }

       
        [Authorize(Roles = "Guest,Host,Admin")]
        [HttpDelete("remove")]
        public async Task<ActionResult> RemoveLike(
            [FromQuery] int userId,
            [FromQuery] LikeTargetType targetType,
            [FromQuery] int? propertyId = null,
            [FromQuery] int? commentId = null)
        {
            var result = await _likeService.RemoveLikeAsync(userId, targetType, propertyId, commentId);
            if (!result)
                return NotFound("Like not found.");

            return Ok(new { message = "Like removed successfully." });
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
    }
}
