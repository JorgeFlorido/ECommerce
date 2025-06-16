using ECommerce.Application.Requests.Queries.Products;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Models;
using MediatR;

namespace ECommerce.Application.Handlers.Products
{
  internal class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Product>>
  {
    private readonly IProductRepository _productRepository;
    public GetAllProductsHandler(IProductRepository productRepository)
    {
      _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }
    public async Task<IEnumerable<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
      var products = await _productRepository.GetAllProductsAsync(cancellationToken);
      return products;
    }
  }
}
