using ECommerce.Application.Interfaces;
using ECommerce.Application.Models;
using ECommerce.Application.Requests.Queries.Orders;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Models.Order;

namespace ECommerce.Application.Services
{
  public class OrderService : IOrderService
  {
    private readonly IOrderRepository _orderRepository;
    private readonly ICheckoutProcessor _checkoutProcessor;
    private readonly IOrderPaymentProcessor _paymentProcessor;

    public OrderService(
        IOrderRepository orderRepository,
        ICheckoutProcessor checkoutProcessor,
        IOrderPaymentProcessor paymentProcessor)
    {
      _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
      _checkoutProcessor = checkoutProcessor ?? throw new ArgumentNullException(nameof(checkoutProcessor));
      _paymentProcessor = paymentProcessor ?? throw new ArgumentNullException(nameof(paymentProcessor));
    }

    public async Task<OrderCostCalculationResult> CalculateOrderCostAsync(OrderCostCalculationQuery query, CancellationToken cancellationToken = default)
    {
      return await _checkoutProcessor.CalculateOrderCostAsync(query, cancellationToken);
    }

    public async Task<CreateOrderResult> CreateOrderAsync(Order order, CancellationToken cancellationToken = default)
    {
      var outOfStockItems = await _checkoutProcessor.GetOutOfStockItemsAsync(order.Items, cancellationToken);
      if (outOfStockItems.Any())
      {
        return new CreateOrderResult
        {
          Success = false,
          OrderId = order.Id,
          OutOfStockItemIds = outOfStockItems
        };
      }

      await _orderRepository.AddOrderAsync(order, cancellationToken);

      return new CreateOrderResult
      {
        Success = true,
        OrderId = order.Id
      };
    }

    public async Task<bool> ProcessOrderAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
      var order = await _orderRepository.GetOrderByIdAsync(orderId, cancellationToken);
      if (order == null)
      {
        return false;
      }

      if (order.Status != OrderStatus.Pending)
      {
        return false;
      }

      try
      {
        var paymentSuccess = await _paymentProcessor.ProcessPaymentAsync(order, cancellationToken);
        if (!paymentSuccess)
        {
          return false;
        }

        order.Status = OrderStatus.Processing;
        await _orderRepository.UpdateOrderAsync(order, cancellationToken);
        return true;
      }
      catch (OperationCanceledException)
      {
        return false;
      }
    }

    public async Task<bool> CancelOrderAsync(Guid orderId, string reason, bool refundPayment, CancellationToken cancellationToken = default)
    {
      var order = await _orderRepository.GetOrderByIdAsync(orderId, cancellationToken);
      if (order == null)
      {
        return false;
      }

      order.Status = OrderStatus.Canceled;
      await _orderRepository.UpdateOrderAsync(order, cancellationToken);
      return true;
    }

    public async Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatus status, string? notes, CancellationToken cancellationToken = default)
    {
      var order = await _orderRepository.GetOrderByIdAsync(orderId, cancellationToken);
      if (order == null)
      {
        return false;
      }

      order.Status = status;
      await _orderRepository.UpdateOrderAsync(order, cancellationToken);
      return true;
    }
  }
}