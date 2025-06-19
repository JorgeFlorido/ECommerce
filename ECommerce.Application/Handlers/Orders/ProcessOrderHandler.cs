using ECommerce.Application.Interfaces;
using ECommerce.Application.Requests.Commands.Orders;
using ECommerce.Domain.Abstractions;
using MediatR;

namespace ECommerce.Application.Handlers.Orders
{
  internal class ProcessOrderHandler : IRequestHandler<ProcessOrderCommand, bool>
  {
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderService _orderService;
    
    public ProcessOrderHandler(
      IOrderRepository orderRepository,
      IOrderService orderService)
    {
      _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
      _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
    }
    
    public async Task<bool> Handle(ProcessOrderCommand request, CancellationToken cancellationToken)
    {
      // Validate order exists
      var order = await _orderRepository.GetOrderByIdAsync(request.OrderId, cancellationToken);
      if (order == null)
      {
        throw new ArgumentException($"Order with ID {request.OrderId} not found.");
      }

      // Use order service for complex business logic
      // This could include:
      // - Payment processing
      // - Inventory management
      // - Shipping label generation
      // - Email notifications
      // - Status updates
      var success = await _orderService.ProcessOrderAsync(request.OrderId, cancellationToken);
      
      return success;
    }
  }
} 