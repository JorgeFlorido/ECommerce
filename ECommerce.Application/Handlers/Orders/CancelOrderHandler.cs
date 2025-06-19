using ECommerce.Application.Interfaces;
using ECommerce.Application.Requests.Commands.Orders;
using ECommerce.Domain.Abstractions;
using MediatR;

namespace ECommerce.Application.Handlers.Orders
{
  internal class CancelOrderHandler : IRequestHandler<CancelOrderCommand, bool>
  {
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderService _orderService;
    
    public CancelOrderHandler(
      IOrderRepository orderRepository,
      IOrderService orderService)
    {
      _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
      _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
    }
    
    public async Task<bool> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
      // Validate order exists
      var order = await _orderRepository.GetOrderByIdAsync(request.OrderId, cancellationToken);
      if (order == null)
      {
        throw new ArgumentException($"Order with ID {request.OrderId} not found.");
      }

      // Use order service for complex business logic
      // This could include:
      // - Payment refund processing
      // - Inventory restoration
      // - Shipping cancellation
      // - Email notifications
      // - Status updates
      var success = await _orderService.CancelOrderAsync(
        request.OrderId, 
        request.Reason, 
        request.RefundPayment, 
        cancellationToken);
      
      return success;
    }
  }
} 