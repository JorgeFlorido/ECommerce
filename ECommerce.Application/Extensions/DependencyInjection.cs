using ECommerce.Application.Factories;
using ECommerce.Application.Handlers.Products;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Services;
using ECommerce.Domain.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ECommerce.Application.Handlers.Orders;

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
      services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
      services.AddScoped<IDomainEventHandler<OrderCreatedEvent>, OrderCreatedEmailHandler>();
      services.AddScoped<IDomainEventHandler<OrderCreatedEvent>, OrderCreatedStockHandler>();

      return services;
    }

    public static IServiceCollection AddFactories(this IServiceCollection services)
    {
      services.AddScoped<IAddressFactory, AddressFactory>();
      return services;
    }
  }
}
