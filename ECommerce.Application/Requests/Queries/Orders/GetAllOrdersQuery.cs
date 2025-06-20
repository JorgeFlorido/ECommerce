using ECommerce.Domain.Models.Order;
using MediatR;

namespace ECommerce.Application.Requests.Queries.Orders
{
  public class GetAllOrdersQuery : IRequest<IEnumerable<Order>>
  {
  }
} 