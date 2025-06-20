using ECommerce.Domain.Models;
using ECommerce.Domain.Models.Orders;

namespace ECommerce.API.Models.Requests.Order
{
  public class OrderCostCalculationRequest
  {
    public Guid CustomerId { get; set; }
    public List<OrderItem> Items { get; set; } = [];
    public OrderShippingAddress? ShippingAddress { get; set; }
    public OrderBillingAddress? BillingAddress { get; set; }
    public string? DiscountCode { get; set; }
    public string? ShippingMethod { get; set; }
  }
}
