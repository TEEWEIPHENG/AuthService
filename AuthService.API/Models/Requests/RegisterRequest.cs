using System.ComponentModel.DataAnnotations;

namespace AuthService.API.Models.Requests
{
    public class RegisterRequest
    {
        [Required, MinLength(3)]
        public string Username { get; set; } = null!;
        [Required]
        public string Firstname { get; set; } = null!;
        [Required]
        public string Lastname { get; set; } = null!;
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required, MinLength(8)]
        public string Password { get; set; } = null!;
        [Required, Phone]
        public string MobileNo { get; set; } = null!;
    }
}
