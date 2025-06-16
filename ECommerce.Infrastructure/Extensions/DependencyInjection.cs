using ECommerce.Database;
using ECommerce.Domain.Abstractions;
using ECommerce.Infrastructure.Identity;
using ECommerce.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

      return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
      services.AddScoped<IProductRepository, ProductRepository>();
      services.AddScoped<IOrderRepository, OrderRepository>();
      services.AddScoped<ICustomerRepository, CustomerRepository>();
      return services;
    }
  }
}
