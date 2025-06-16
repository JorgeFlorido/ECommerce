using Microsoft.AspNetCore.Identity;

namespace ECommerce.Infrastructure.Identity
{
  public static class IdentityDataSeeder
  {
    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
      var roles = new[] { "Admin", "User", "Guest" };
      foreach (var role in roles)
      { 
        if (!await roleManager.RoleExistsAsync(role))
        {
          await roleManager.CreateAsync(new IdentityRole(role));
        }
      }
    }
  }
}
