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
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var currentRole = User.FindFirstValue(ClaimTypes.Role);

            if (currentUserId != id && currentRole != "Admin")
                return Forbid("You are not authorized to view this user.");

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound($"User with ID {id} not found.");

            return Ok(user);
        }

       
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.CreateUserAsync(createUserDto);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        
        [Authorize(Roles = "Guest,Host,Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] CreateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var currentRole = User.FindFirstValue(ClaimTypes.Role);

            if (currentUserId != id && currentRole != "Admin")
                return Forbid("You are not authorized to update this user.");

            var user = await _userService.UpdateUserAsync(id, updateUserDto);
            if (user == null)
                return NotFound($"User with ID {id} not found.");

            return Ok(user);
        }

        
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
                return NotFound($"User with ID {id} not found.");

            return NoContent();
        }

        
        [Authorize(Roles = "Admin")]
        [HttpGet("email/{email}")]
        public async Task<ActionResult<UserDto>> GetUserByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
                return NotFound($"User with email {email} not found.");
            return Ok(user);
        }

        
        [Authorize(Roles = "Admin")]
        [HttpGet("username/{username}")]
        public async Task<ActionResult<UserDto>> GetUserByUsername(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null)
                return NotFound($"User with username {username} not found.");
            return Ok(user);
        }

        
        [AllowAnonymous]
        [HttpGet("email-exists/{email}")]
        public async Task<ActionResult<bool>> EmailExists(string email)
        {
            var exists = await _userService.EmailExistsAsync(email);
            return Ok(exists);
        }

        [AllowAnonymous]
        [HttpGet("username-exists/{username}")]
        public async Task<ActionResult<bool>> UsernameExists(string username)
        {
            var exists = await _userService.UsernameExistsAsync(username);
            return Ok(exists);
        }
    }
}
