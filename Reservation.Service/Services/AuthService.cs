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
            // Ellenőrizzük, hogy az email már létezik-e
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                throw new InvalidOperationException("Email already exists.");

            // Ellenőrizzük, hogy a username már létezik-e
            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
                throw new InvalidOperationException("Username already exists.");

            // Jelszó hash-elés
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            // Új felhasználó létrehozása — mindig Guest
            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                Role = RoleType.Admin // 🔹 Fix: mindig Guest
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // JWT token generálása
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
            // Felhasználó keresése username vagy email alapján
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginDto.Email || u.Email == loginDto.Email);

            if (user == null)
                throw new InvalidOperationException("Invalid username or password.");

            // Jelszó ellenőrzése
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                throw new InvalidOperationException("Invalid username or password.");

            // JWT token generálása
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

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            // Jelenlegi jelszó ellenőrzése
            if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.PasswordHash))
                throw new InvalidOperationException("Current password is incorrect.");

            // Új jelszó hash-elése és mentése
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "YourSecretKeyThatIsAtLeast32CharactersLong!");

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "YourSecretKeyThatIsAtLeast32CharactersLong!");

                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = false, // Nem ellenőrizzük a lejárati időt refresh-nél
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                    throw new InvalidOperationException("Invalid token.");

                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    throw new InvalidOperationException("User not found.");

                // Új token generálása
                var newToken = GenerateJwtToken(user);

                return new AuthResponseDto
                {
                    UserId = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    Token = newToken
                };
            }
            catch
            {
                throw new InvalidOperationException("Invalid token.");
            }
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
                Expires = DateTime.UtcNow.AddDays(7), // 7 napos token
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
