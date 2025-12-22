using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Reservation.DataContext.Dtos;
using Reservation.Service.Interfaces;

namespace Reservation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        
        [Authorize(Roles = "Guest,Host,Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var currentUserId = GetCurrentUserId();
            var currentRole = User.FindFirstValue(ClaimTypes.Role);

            if (currentUserId != id && currentRole != "Admin")
                return Forbid();

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound($"User with ID {id} not found.");

            return Ok(user);
        }

        [Authorize] 
        [HttpGet("profile")]
        public async Task<ActionResult<UserDto>> GetMyProfile()
        {
            var currentUserId = GetCurrentUserId(); 
            var user = await _userService.GetUserByIdAsync(currentUserId);

            if (user == null) return NotFound("User profile not found.");

            return Ok(user);
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<ActionResult<UserDto>> UpdateMyProfile([FromBody] UpdateUserProfileDto request)
        {
            var currentUserId = GetCurrentUserId(); 

            
            var updatedUser = await _userService.UpdateUserProfileAsync(currentUserId, request);

            if (updatedUser == null) return NotFound("User not found.");

            return Ok(updatedUser);
        }

        
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = await _userService.CreateUserAsync(createUserDto);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [Authorize(Roles = "Guest,Host,Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] CreateUserDto updateUserDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var currentUserId = GetCurrentUserId();
            var currentRole = User.FindFirstValue(ClaimTypes.Role);

            if (currentUserId != id && currentRole != "Admin")
                return Forbid();

            var user = await _userService.UpdateUserAsync(id, updateUserDto);
            if (user == null) return NotFound($"User with ID {id} not found.");

            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result) return NotFound($"User with ID {id} not found.");
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/trust")]
        public async Task<ActionResult> SetTrustedHost(int id, [FromQuery] bool isTrusted)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound($"User with ID {id} not found.");

            await _userService.SetTrustedStatusAsync(id, isTrusted);
            return Ok(new { message = $"User {(isTrusted ? "marked as trusted" : "untrusted")} host." });
        }

        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(idClaim)) return 0;
            return int.Parse(idClaim);
        }
    }
}