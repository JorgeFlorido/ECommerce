using ECommerce.Application.Models.Authentication;

namespace ECommerce.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> AuthenticateAsync(string email, string password);
        Task<AuthenticationResult> RefreshTokenAsync(string accessToken, string refreshToken);
    }
} 