using ECommerce.Application.Requests.Queries.Orders;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Models.Order;
using MediatR;

namespace ECommerce.Application.Handlers.Orders
{
  internal class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, Order?>
  {
    private readonly IOrderRepository _orderRepository;
    
    public GetOrderByIdHandler(IOrderRepository orderRepository)
    {
      _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    }
    
    public async Task<Order?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
      var order = await _orderRepository.GetOrderByIdAsync(request.Id, cancellationToken);
      return order;
    }
  }
} 