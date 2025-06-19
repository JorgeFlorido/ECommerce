using ECommerce.Application.Requests.Queries.Orders;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Models.Orders;
using MediatR;

namespace ECommerce.Application.Handlers.Orders
{
  internal class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<Order>>
  {
    private readonly IOrderRepository _orderRepository;
    
    public GetAllOrdersHandler(IOrderRepository orderRepository)
    {
      _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    }
    
    public async Task<IEnumerable<Order>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
      var orders = await _orderRepository.GetAllOrdersAsync(cancellationToken);
      return orders;
    }
  }
} 