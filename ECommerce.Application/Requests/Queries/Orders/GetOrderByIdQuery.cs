using ECommerce.Domain.Models.Order;
using MediatR;

namespace ECommerce.Application.Requests.Queries.Orders
{
  public class GetOrderByIdQuery : IRequest<Order?>
  {
    public Guid Id { get; set; }
  }
} 