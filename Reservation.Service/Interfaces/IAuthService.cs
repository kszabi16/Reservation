using Reservation.DataContext.Dtos;

namespace Reservation.Service.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
        Task<bool> ValidateTokenAsync(string token);
        Task<AuthResponseDto> RefreshTokenAsync(string token);
    }
}
