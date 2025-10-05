using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetAllLikes()
        {
            try
            {
                var likes = await _likeService.GetAllLikesAsync();
                return Ok(likes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LikeDto>> GetLike(int id)
        {
            try
            {
                var like = await _likeService.GetLikeByIdAsync(id);
                if (like == null)
                    return NotFound($"Like with ID {id} not found.");

                return Ok(like);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<LikeDto>> CreateLike([FromBody] CreateLikeDto createLikeDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var like = await _likeService.CreateLikeAsync(createLikeDto);
                return CreatedAtAction(nameof(GetLike), new { id = like.Id }, like);
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
        public async Task<ActionResult> DeleteLike(int id)
        {
            try
            {
                var result = await _likeService.DeleteLikeAsync(id);
                if (!result)
                    return NotFound($"Like with ID {id} not found.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetLikesByUser(int userId)
        {
            try
            {
                var likes = await _likeService.GetLikesByUserIdAsync(userId);
                return Ok(likes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("property/{propertyId}")]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetLikesByProperty(int propertyId)
        {
            try
            {
                var likes = await _likeService.GetLikesByPropertyIdAsync(propertyId);
                return Ok(likes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("comment/{commentId}")]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetLikesByComment(int commentId)
        {
            try
            {
                var likes = await _likeService.GetLikesByCommentIdAsync(commentId);
                return Ok(likes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("target-type/{targetType}")]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetLikesByTargetType(LikeTargetType targetType)
        {
            try
            {
                var likes = await _likeService.GetLikesByTargetTypeAsync(targetType);
                return Ok(likes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("is-liked")]
        public async Task<ActionResult<bool>> IsLiked(
            [FromQuery] int userId, 
            [FromQuery] LikeTargetType targetType,
            [FromQuery] int? propertyId = null,
            [FromQuery] int? commentId = null)
        {
            try
            {
                var isLiked = await _likeService.IsLikedAsync(userId, targetType, propertyId, commentId);
                return Ok(isLiked);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("remove")]
        public async Task<ActionResult> RemoveLike(
            [FromQuery] int userId,
            [FromQuery] LikeTargetType targetType,
            [FromQuery] int? propertyId = null,
            [FromQuery] int? commentId = null)
        {
            try
            {
                var result = await _likeService.RemoveLikeAsync(userId, targetType, propertyId, commentId);
                if (!result)
                    return NotFound("Like not found.");

                return Ok(new { message = "Like removed successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("toggle")]
        public async Task<ActionResult<bool>> ToggleLike(
            [FromQuery] int userId,
            [FromQuery] LikeTargetType targetType,
            [FromQuery] int? propertyId = null,
            [FromQuery] int? commentId = null)
        {
            try
            {
                var isLiked = await _likeService.ToggleLikeAsync(userId, targetType, propertyId, commentId);
                return Ok(new { isLiked, message = isLiked ? "Liked." : "Unliked." });
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
