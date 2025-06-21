using ECommerce.Application.Requests.Queries.Products;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Models;
using ECommerce.Domain.Models.Product;
using MediatR;

namespace ECommerce.Application.Handlers.Products
{
  internal class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, PagedList<Product>>
  {
    private readonly IProductRepository _productRepository;
    public GetAllProductsHandler(IProductRepository productRepository)
    {
      _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }
    public async Task<PagedList<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
      var products = await _productRepository.GetAllProductsAsync(request.Filter, request.Pagination, cancellationToken);
      return products;
    }
  }
}
