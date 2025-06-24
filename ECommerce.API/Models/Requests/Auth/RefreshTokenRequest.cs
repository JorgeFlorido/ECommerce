using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.Models.Requests.Auth
{
    public class RefreshTokenRequest
    {
        [Required]
        public string AccessToken { get; set; } = null!;

        [Required]
        public string RefreshToken { get; set; } = null!;
    }
} 