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
            => Ok(await _commentService.GetCommentsByPropertyIdAsync(propertyId));

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CommentDto>> CreateComment([FromBody] CreateCommentDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            dto.UserId = userId;
            var comment = await _commentService.CreateCommentAsync(dto);
            return CreatedAtAction(nameof(GetCommentsByProperty), new { propertyId = dto.PropertyId }, comment);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComment(int id)
        {
            var result = await _commentService.DeleteCommentAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}
