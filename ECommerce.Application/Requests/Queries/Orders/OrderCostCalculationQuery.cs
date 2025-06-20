using ECommerce.Application.Models;
using ECommerce.Domain.Models;
using ECommerce.Domain.Models.Order;
using MediatR;

namespace ECommerce.Application.Requests.Queries.Orders
{
  public class OrderCostCalculationQuery : IRequest<OrderCostCalculationResult>
  {
    public Guid CustomerId { get; set; }
    public List<OrderItem> Items { get; set; } = [];
    public OrderShippingAddress? ShippingAddress { get; set; }
    public OrderBillingAddress? BillingAddress { get; set; }
    public string? DiscountCode { get; set; }
    public string? ShippingMethod { get; set; }
  }
}
