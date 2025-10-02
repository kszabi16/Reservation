using Reservation.DataContext.Entities;

namespace Reservation.DataContext.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public RoleType Role { get; set; }
    }

    public class CreateUserDto
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;  // majd hash
        public RoleType Role { get; set; }
    }


}
