using ECommerce.Application.Models;
using ECommerce.Application.Requests.Queries.Orders;
using ECommerce.Domain.Models.Order;

namespace ECommerce.Application.Interfaces
{
  public interface ICheckoutProcessor
  {
    Task<OrderCostCalculationResult> CalculateOrderCostAsync(OrderCostCalculationQuery query, CancellationToken ct);
    Task<List<Guid>> GetOutOfStockItemsAsync(IEnumerable<OrderItem> items, CancellationToken ct);
  }
}
