using ECommerce.Domain.Models;
using MediatR;

namespace ECommerce.Application.Requests.Queries.Products
{
  public class GetProductByIdQuery : IRequest<Product>
  {
    public Guid Id { get; set; }
  }
}
