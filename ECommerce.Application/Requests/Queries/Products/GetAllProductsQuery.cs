using ECommerce.Domain.Models;
using MediatR;

namespace ECommerce.Application.Requests.Queries.Products
{
  public class GetAllProductsQuery : IRequest<IEnumerable<Product>>
  {
  }
}
