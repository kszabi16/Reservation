using Reservation.DataContext.Entities;
using System.ComponentModel.DataAnnotations;

namespace Reservation.DataContext.Dtos
{
    public class RegisterDto
    {
        public string Username { get; set; } = null!;
        [Required(ErrorMessage = "Az email kötelező!")]
        [EmailAddress(ErrorMessage = "Érvénytelen email formátum!")]
        public string Email { get; set; } = null!;
        [Required]
        [MinLength(6, ErrorMessage = "A jelszónak legalább 6 karakternek kell lennie!")]
        public string Password { get; set; } = null!;
    }

    public class LoginDto
    {
        [Required(ErrorMessage = "Az email kötelező!")]
        [EmailAddress(ErrorMessage = "Érvénytelen email formátum!")]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }

    public class AuthResponseDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public RoleType Role { get; set; }
        public string Token { get; set; } = null!;
    }
}
