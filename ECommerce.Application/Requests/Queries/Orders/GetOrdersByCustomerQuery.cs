using ECommerce.Domain.Models.Order;
using MediatR;

namespace ECommerce.Application.Requests.Queries.Orders
{
  public class GetOrdersByCustomerQuery : IRequest<IEnumerable<Order>>
  {
    public Guid CustomerId { get; set; }
  }
} 