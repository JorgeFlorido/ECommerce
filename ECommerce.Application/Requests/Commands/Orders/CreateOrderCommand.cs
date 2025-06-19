using ECommerce.Application.Requests.Commands.Addresses;
using MediatR;

namespace ECommerce.Application.Requests.Commands.Orders
{
  public class CreateOrderCommand : IRequest<Guid>
  {
    public Guid CustomerId { get; set; }
    public List<OrderItemCommand> Items { get; set; } = [];
    public OrderShippingAddressCommand? ShippingAddress { get; set; }
    public OrderBillingAddressCommand? BillingAddress { get; set; }
  }

  public class OrderItemCommand
  {
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
  }
} 