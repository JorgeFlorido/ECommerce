using ECommerce.Application.Handlers.Products;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application.Extensions
{
  public static class DependencyInjection
  {
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
      services.AddMediatR(cfg =>
          cfg.RegisterServicesFromAssemblies(typeof(AddProductHandler).Assembly));
      return services;
    }
  }
}
