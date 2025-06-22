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

    public async Task<OrderCostCalculationResult> CalculateOrderCostAsync(OrderCostCalculationQuery query, CancellationToken ct = default)
    {
      return await _checkoutProcessor.CalculateOrderCostAsync(query, ct);
    }

    public async Task<CreateOrderResult> CreateOrderAsync(Order order, CancellationToken ct = default)
    {
      var query = new OrderCostCalculationQuery
      {
        CustomerId = order.CustomerId,
        Items = order.Items,
        ShippingAddress = order.ShippingAddress,
        BillingAddress = order.BillingAddress,
        DiscountCode = order.DiscountCode?.Code
      };

      var costResult = await _checkoutProcessor.CalculateOrderCostAsync(query, ct);

      order.GrossAmount = costResult.GrossAmount;
      order.TaxAmount = costResult.TaxAmount;
      order.ShippingCost = costResult.ShippingCost;
      order.DiscountCode = costResult.DiscountCode;
      order.OtherFees = costResult.OtherFees;

      var outOfStock = await _checkoutProcessor.GetOutOfStockItemsAsync(order.Items, ct);
      if (outOfStock.Count > 0)
      {
        return new CreateOrderResult
        {
          Success = false,
          OutOfStockItemIds = outOfStock,
          Message = "Some items are out of stock."
        };
      }

      await _orderRepository.AddOrderAsync(order, ct);
      return new CreateOrderResult
      {
        Success = true,
        OrderId = order.Id
      };
    }

    public async Task<bool> ProcessOrderAsync(Guid orderId, CancellationToken ct = default)
    {
      var order = await _orderRepository.GetOrderByIdAsync(orderId, ct);
      if (order == null) return false;

      var paymentSuccess = await _paymentProcessor.ProcessPaymentAsync(order, ct);
      if (!paymentSuccess) return false;

      order.Status = OrderStatus.Processing;
      await _orderRepository.UpdateOrderAsync(order, ct);
      return true;
    }

    public async Task<bool> CancelOrderAsync(Guid orderId, string? reason, bool refundPayment, CancellationToken ct = default)
    {
      var order = await _orderRepository.GetOrderByIdAsync(orderId, ct);
      if (order == null) return false;

      if (refundPayment)
      {
        var refundSuccess = await _paymentProcessor.RefundPaymentAsync(order, ct);
        if (!refundSuccess) return false;
      }

      order.Status = OrderStatus.Canceled;
      await _orderRepository.UpdateOrderAsync(order, ct);
      return true;
    }

    public async Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatus status, string? notes, CancellationToken ct = default)
    {
      var order = await _orderRepository.GetOrderByIdAsync(orderId, ct);
      if (order == null) return false;

      order.Status = status;
      await _orderRepository.UpdateOrderAsync(order, ct);
      return true;
    }
  }
}