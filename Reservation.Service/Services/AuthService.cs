using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Reservation.DataContext.Context;
using Reservation.DataContext.Dtos;
using Reservation.DataContext.Entities;
using Reservation.Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;

namespace Reservation.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly ReservationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(IMapper mapper, ReservationDbContext context, IConfiguration configuration)
        {
            _mapper = mapper;
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {

            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                throw new InvalidOperationException("Email already exists.");

            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
                throw new InvalidOperationException("Username already exists.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                Role = RoleType.Guest
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Token = token
            };
        }


        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginDto.Email || u.Email == loginDto.Email);

            if (user == null)
                throw new InvalidOperationException("Invalid username or password.");

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                throw new InvalidOperationException("Invalid username or password.");

            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Token = token
            };
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "YourSecretKeyThatIsAtLeast32CharactersLong!");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
