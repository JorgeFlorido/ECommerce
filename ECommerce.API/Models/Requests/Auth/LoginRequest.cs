using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.Models.Requests.Auth
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
} 