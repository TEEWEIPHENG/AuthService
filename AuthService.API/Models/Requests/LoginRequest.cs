using System.ComponentModel.DataAnnotations;

namespace AuthService.API.Models.Requests
{
    public class LoginRequest
    {
        [Required]
        public string Credential {get;set;}
        [Required]
        public string Password {get;set;}
    }
}