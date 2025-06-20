using ECommerce.Application.Models;
using ECommerce.Application.Requests.Queries.Orders;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Models.Order;

namespace ECommerce.Application.Interfaces
{
  public interface IOrderService
  {
    Task<OrderCostCalculationResult> CalculateOrderCostAsync(OrderCostCalculationQuery orderCostCalculationQuery, CancellationToken cancellationToken = default);
    Task<CreateOrderResult> CreateOrderAsync(Order order, CancellationToken cancellationToken = default);
    Task<bool> ProcessOrderAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<bool> CancelOrderAsync(Guid orderId, string? reason, bool refundPayment, CancellationToken cancellationToken = default);
    Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatus status, string? notes, CancellationToken cancellationToken = default);
  }
} 