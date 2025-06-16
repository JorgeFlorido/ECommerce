using ECommerce.Application.Requests.Commands.Products;
using ECommerce.Domain.Abstractions;
using MediatR;

namespace ECommerce.Application.Handlers.Products
{
  public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool> 
  {
    private readonly IProductRepository _productRepository;

    public DeleteProductHandler(IProductRepository productRepository)
    {
      _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
      var product = await _productRepository.GetProductByIdAsync(request.ProductId, cancellationToken);
      if (product == null)
        return false;

      await _productRepository.DeleteProductAsync(request.ProductId, cancellationToken);
      return true;
    }
  }
}
