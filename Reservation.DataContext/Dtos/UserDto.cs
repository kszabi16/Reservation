using Reservation.DataContext.Entities;

namespace Reservation.DataContext.Dtos
{
    
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public RoleType Role { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Location { get; set; }
        public string? Bio { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UpdateUserProfileDto
    {
        public string? Username { get; set; } 
        public string? PhoneNumber { get; set; }
        public string? Location { get; set; }
        public string? Bio { get; set; }
    }
    
    public class CreateUserDto
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public RoleType Role { get; set; }
    }
}