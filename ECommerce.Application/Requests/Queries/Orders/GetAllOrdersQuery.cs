using ECommerce.Domain.Models.Orders;
using MediatR;

namespace ECommerce.Application.Requests.Queries.Orders
{
  public class GetAllOrdersQuery : IRequest<IEnumerable<Order>>
  {
  }
} 