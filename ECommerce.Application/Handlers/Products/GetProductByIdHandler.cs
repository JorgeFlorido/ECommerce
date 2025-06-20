using ECommerce.Application.Requests.Queries.Products;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Models.Product;
using MediatR;

namespace ECommerce.Application.Handlers.Products
{
  internal class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Product>
  {
    private readonly IProductRepository _productRepository;

    public GetProductByIdHandler(IProductRepository productRepository)
    {
      _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
      var product = await _productRepository.GetProductByIdAsync(request.Id, cancellationToken);
      return product ?? throw new KeyNotFoundException($"Product with ID {request.Id} not found.");
    }
  }
}
