using ECommerce.Domain.Models.User;
using System.Security.Claims;

namespace ECommerce.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(BaseUser user, IEnumerable<string> roles);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
} 