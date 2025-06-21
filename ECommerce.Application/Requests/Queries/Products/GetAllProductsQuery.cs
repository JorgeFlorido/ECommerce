using ECommerce.Domain.Common.Models;
using ECommerce.Domain.Models;
using ECommerce.Domain.Models.Product;
using MediatR;

namespace ECommerce.Application.Requests.Queries.Products
{
  public class GetAllProductsQuery : IRequest<PagedList<Product>>
  {
    public ProductFilterQuery Filter { get; set; } = new();
    public PaginationQuery Pagination { get; set; } = new();
  }
}
