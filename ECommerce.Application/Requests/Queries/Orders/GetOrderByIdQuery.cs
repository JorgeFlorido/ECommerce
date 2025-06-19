using ECommerce.Domain.Models.Orders;
using MediatR;

namespace ECommerce.Application.Requests.Queries.Orders
{
  public class GetOrderByIdQuery : IRequest<Order?>
  {
    public Guid Id { get; set; }
  }
} 