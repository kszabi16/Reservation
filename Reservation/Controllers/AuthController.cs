using Microsoft.AspNetCore.Mvc;
using Reservation.DataContext.Dtos;
using Reservation.Service.Interfaces;

namespace Reservation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Megpróbáljuk beregisztrálni
                var user = await _authService.RegisterAsync(dto); // vagy ahogy nálad hívják a metódust
                return Ok(new { message = "Sikeres regisztráció!" });
            }
            catch (InvalidOperationException ex)
            {
                // HA LÉTEZIK MÁR AZ EMAIL VAGY USERNAME, ITT KAPJUK EL!
                // Visszaadunk egy 400 Bad Request-et a konkrét hibaüzenettel.
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                // Bármilyen egyéb váratlan hiba
                return StatusCode(500, new { message = "Belsõ szerverhiba történt a regisztráció során." });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _authService.LoginAsync(loginDto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpPost("change-password/{userId}")]
        public async Task<ActionResult> ChangePassword(int userId, [FromBody] ChangePasswordDto changePasswordDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _authService.ChangePasswordAsync(userId, changePasswordDto);
                if (!result)
                    return NotFound(new { message = "User not found." });

                return Ok(new { message = "Password changed successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpPost("validate-token")]
        public async Task<ActionResult<bool>> ValidateToken([FromBody] string token)
        {
            try
            {
                var isValid = await _authService.ValidateTokenAsync(token);
                return Ok(new { isValid });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] string token)
        {
            try
            {
                var result = await _authService.RefreshTokenAsync(token);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }
    }
}
