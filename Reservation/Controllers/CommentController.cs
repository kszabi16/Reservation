using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetAllComments()
        {
            try
            {
                var comments = await _commentService.GetAllCommentsAsync();
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CommentDto>> GetComment(int id)
        {
            try
            {
                var comment = await _commentService.GetCommentByIdAsync(id);
                if (comment == null)
                    return NotFound($"Comment with ID {id} not found.");

                return Ok(comment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CommentDto>> CreateComment([FromBody] CreateCommentDto createCommentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var comment = await _commentService.CreateCommentAsync(createCommentDto);
                return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, comment);
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

        [HttpPut("{id}")]
        public async Task<ActionResult<CommentDto>> UpdateComment(int id, [FromBody] CreateCommentDto updateCommentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var comment = await _commentService.UpdateCommentAsync(id, updateCommentDto);
                if (comment == null)
                    return NotFound($"Comment with ID {id} not found.");

                return Ok(comment);
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
        public async Task<ActionResult> DeleteComment(int id)
        {
            try
            {
                var result = await _commentService.DeleteCommentAsync(id);
                if (!result)
                    return NotFound($"Comment with ID {id} not found.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("property/{propertyId}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsByProperty(int propertyId)
        {
            try
            {
                var comments = await _commentService.GetCommentsByPropertyIdAsync(propertyId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsByUser(int userId)
        {
            try
            {
                var comments = await _commentService.GetCommentsByUserIdAsync(userId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{parentCommentId}/replies")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentReplies(int parentCommentId)
        {
            try
            {
                var replies = await _commentService.GetCommentRepliesAsync(parentCommentId);
                return Ok(replies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("property/{propertyId}/top-level")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetTopLevelComments(int propertyId)
        {
            try
            {
                var comments = await _commentService.GetTopLevelCommentsAsync(propertyId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
