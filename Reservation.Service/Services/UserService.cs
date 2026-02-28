using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reservation.DataContext.Context;
using Reservation.DataContext.Dtos;
using Reservation.DataContext.Entities;
using Reservation.Service.Interfaces;
using BCrypt.Net;

namespace Reservation.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly ReservationDbContext _context;

        public UserService(IMapper mapper, ReservationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            if (await EmailExistsAsync(createUserDto.Email))
                throw new InvalidOperationException("Email already exists.");

            if (await UsernameExistsAsync(createUserDto.Username))
                throw new InvalidOperationException("Username already exists.");

            // Jelszó hash-elés
            createUserDto.Password = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

            var user = _mapper.Map<User>(createUserDto);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> UpdateUserAsync(int id, CreateUserDto updateUserDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return null;

            // Ellenőrizzük, hogy az email/username már létezik-e másik felhasználónál
            if (user.Email != updateUserDto.Email && await EmailExistsAsync(updateUserDto.Email))
                throw new InvalidOperationException("Email already exists.");

            if (user.Username != updateUserDto.Username && await UsernameExistsAsync(updateUserDto.Username))
                throw new InvalidOperationException("Username already exists.");

            _mapper.Map(updateUserDto, user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            user.Deleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetUserByUsernameAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }
        public async Task SetTrustedStatusAsync(int userId, bool isTrusted)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            user.IsTrustedHost = isTrusted;
            await _context.SaveChangesAsync();
        }

        public async Task<UserDto?> UpdateUserProfileAsync(int id, UpdateUserProfileDto profileDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;
            _mapper.Map(profileDto, user);

            await _context.SaveChangesAsync();
            
            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> UpdateUserRoleAsync(int id, RoleType newRole)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.Role = newRole;
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
