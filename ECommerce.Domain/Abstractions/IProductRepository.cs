using ECommerce.Domain.Common.Models;
using ECommerce.Domain.Models;
using ECommerce.Domain.Models.Product;

namespace ECommerce.Domain.Abstractions
{
  public interface IProductRepository
  {
    Task<PagedList<Product>> GetAllProductsAsync(ProductFilterQuery filter, PaginationQuery pagination, CancellationToken cancellationToken = default);
    Task<Product?> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task AddProductAsync(Product product, CancellationToken cancellationToken = default);
    Task UpdateProductAsync(Product product, CancellationToken cancellationToken = default);
    Task DeleteProductAsync(Guid productId, CancellationToken cancellationToken = default);
  }
}
