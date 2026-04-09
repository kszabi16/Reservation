using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Reservation.DataContext.Dtos;
using Reservation.Service.Interfaces;

namespace Reservation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        
        [HttpGet("property/{propertyId}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsByProperty(int propertyId)
        {
            var comments = await _commentService.GetCommentsByPropertyIdAsync(propertyId);

            return Ok(comments);
        }

       
        [Authorize(Roles = "Guest,Host,Admin")]
        [HttpPost]
        public async Task<ActionResult<CommentDto>> CreateComment([FromBody] CreateCommentDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var comment = await _commentService.CreateCommentAsync(dto,userId);

            return CreatedAtAction(nameof(GetCommentsByProperty), new { propertyId = dto.PropertyId }, comment);
        }

        
        [Authorize(Roles = "Guest,Host,Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComment(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var deleted = await _commentService.DeleteCommentAsync(id);

            return deleted ? NoContent() : Forbid("You are not authorized to delete this comment.");
        }

    }
}
