using ECommerce.Domain.Enums;
using ECommerce.Domain.Models.Orders;

namespace ECommerce.Application.Interfaces
{
  public interface IOrderService
  {
    Task<Guid> CreateOrderAsync(Order order, CancellationToken cancellationToken = default);
    Task<bool> ProcessOrderAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<bool> CancelOrderAsync(Guid orderId, string? reason, bool refundPayment, CancellationToken cancellationToken = default);
    Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatus status, string? notes, CancellationToken cancellationToken = default);
  }
} 