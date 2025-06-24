using Microsoft.AspNetCore.Identity;
using ECommerce.Application.Common.Constants;

namespace ECommerce.Infrastructure.Identity
{
  public static class IdentityDataSeeder
  {
    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
      var roles = new[] { AppRoles.Admin, AppRoles.Customer, AppRoles.Guest };
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
