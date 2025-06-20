using ECommerce.Application.Interfaces;
using ECommerce.Application.Models;
using ECommerce.Application.Requests.Queries.Orders;
using MediatR;

namespace ECommerce.Application.Handlers.Orders
{
  public class OrderCostCalculationHandler : IRequestHandler<OrderCostCalculationQuery, OrderCostCalculationResult>
  {
    private readonly IOrderService _orderService;

    public OrderCostCalculationHandler(IOrderService orderService)
    {
      _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
    }

    public Task<OrderCostCalculationResult> Handle(OrderCostCalculationQuery request, CancellationToken cancellationToken)
    {
      return _orderService.CalculateOrderCostAsync(request, cancellationToken);
    }
  }
}
