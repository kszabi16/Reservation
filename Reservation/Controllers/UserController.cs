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

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> GetMe()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _userService.GetUserByIdAsync(userId);
            return user is null ? NotFound() : Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
            => Ok(await _userService.GetAllUsersAsync());
    }
}
