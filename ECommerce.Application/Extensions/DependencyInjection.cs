using ECommerce.Application.Factories;
using ECommerce.Application.Handlers.Products;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Services;
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

      services.AddScoped<IOrderService, OrderService>();
      services.AddScoped<INotificationService, NotificationService>();

      return services;
    }

    public static IServiceCollection AddFactories(this IServiceCollection services)
    {
      services.AddScoped<IAddressFactory, AddressFactory>();
      return services;
    }
  }
}
