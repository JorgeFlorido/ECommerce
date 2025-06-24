using ECommerce.Database;
using ECommerce.Domain.Abstractions;
using ECommerce.Infrastructure.Identity;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Application.Interfaces;
using ECommerce.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Infrastructure.Extensions
{
  public static class DependencyInjection
  {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddDbContext<AppDbContext>(options =>
          options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

      services.AddDbContext<AppIdentityDbContext>(options =>
          options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

      services.AddIdentity<AppUser, IdentityRole>(options =>
      {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 8;
        options.Password.RequiredUniqueChars = 1;

        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = true;
      })
      .AddEntityFrameworkStores<AppIdentityDbContext>()
      .AddDefaultTokenProviders();

      services.AddScoped<ITokenService, JwtTokenService>();
      services.AddScoped<IAuthenticationService, AuthenticationService>();

      return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
      services.AddScoped<IProductRepository, ProductRepository>();
      services.AddScoped<IOrderRepository, OrderRepository>();
      services.AddScoped<ICustomerRepository, CustomerRepository>();
      return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
      services.AddScoped<IEmailService, MailKitEmailService>();
      services.AddScoped<ISmsService, TwilioSmsService>();
      return services;
    }
  }
}
