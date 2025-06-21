using ECommerce.Domain.Common.Models;

namespace ECommerce.API.Models.Requests.Product
{
  public class GetAllProductsRequest
  {
    public ProductFilterQuery Filter { get; set; } = new();
    public PaginationQuery Pagination { get; set; } = new();
  }
} 