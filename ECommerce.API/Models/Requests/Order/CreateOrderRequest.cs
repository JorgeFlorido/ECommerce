using ECommerce.API.Models.Requests.Address;

namespace ECommerce.API.Models.Requests.Order
{
  public class CreateOrderRequest
  {
    public Guid CustomerId { get; set; }
    public List<OrderItemRequest> Items { get; set; } = [];
    public OrderShippingAddressRequest? ShippingAddress { get; set; }
    public OrderBillingAddressRequest? BillingAddress { get; set; }
  }
} 