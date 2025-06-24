using Microsoft.AspNetCore.Identity;

namespace ECommerce.Infrastructure.Identity
{
  public class AppUser : IdentityUser
  {
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
  }
}
