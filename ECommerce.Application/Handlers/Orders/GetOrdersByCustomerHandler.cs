using ECommerce.Application.Requests.Queries.Orders;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Models.Order;
using MediatR;

namespace ECommerce.Application.Handlers.Orders
{
  internal class GetOrdersByCustomerHandler : IRequestHandler<GetOrdersByCustomerQuery, IEnumerable<Order>>
  {
    private readonly IOrderRepository _orderRepository;
    
    public GetOrdersByCustomerHandler(IOrderRepository orderRepository)
    {
      _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    }
    
    public async Task<IEnumerable<Order>> Handle(GetOrdersByCustomerQuery request, CancellationToken cancellationToken)
    {
      var orders = await _orderRepository.GetOrdersByCustomerIdAsync(request.CustomerId, cancellationToken);
      return orders;
    }
  }
} 