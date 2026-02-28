using Reservation.DataContext.Dtos;
using Reservation.DataContext.Entities;

namespace Reservation.Service.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<UserDto?> UpdateUserAsync(int id, CreateUserDto updateUserDto);
        Task<bool> DeleteUserAsync(int id);
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task<UserDto?> GetUserByUsernameAsync(string username);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> UsernameExistsAsync(string username);
        Task SetTrustedStatusAsync(int userId, bool isTrusted);
        Task<bool> UpdateUserRoleAsync(int id, RoleType newRole);
        Task<UserDto?> UpdateUserProfileAsync(int id, UpdateUserProfileDto profileDto);

    }
}
