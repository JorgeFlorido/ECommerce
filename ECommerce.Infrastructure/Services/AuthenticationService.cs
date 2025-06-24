using ECommerce.Application.Interfaces;
using ECommerce.Application.Models.Authentication;
using ECommerce.Domain.Models.User;
using ECommerce.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public AuthenticationService(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<AuthenticationResult> AuthenticateAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new AuthenticationResult { Success = false, Error = "User not found" };
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordValid)
            {
                return new AuthenticationResult { Success = false, Error = "Invalid password" };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var customer = MapToCustomer(user);
            var accessToken = _tokenService.GenerateToken(customer, roles);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return new AuthenticationResult
            {
                Success = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return new AuthenticationResult { Success = false, Error = "Invalid access token" };
            }

            var userId = principal.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return new AuthenticationResult { Success = false, Error = "Invalid access token" };
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return new AuthenticationResult { Success = false, Error = "Invalid refresh token" };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var customer = MapToCustomer(user);
            var newAccessToken = _tokenService.GenerateToken(customer, roles);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return new AuthenticationResult
            {
                Success = true,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        private static Customer MapToCustomer(AppUser user)
        {
            var customer = new Customer();
            var type = typeof(Customer);
            var idProperty = type.GetProperty(nameof(Customer.Id));
            var emailProperty = type.GetProperty(nameof(Customer.Email));

            idProperty?.SetValue(customer, Guid.Parse(user.Id));
            emailProperty?.SetValue(customer, user.Email);

            return customer;
        }
    }
} 